namespace Aardvark.Rendering.Text

open Aardvark.Base
open System
open System.IO
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop
open System.Security
open FuzzySharp
open Typography.OpenFont

#nowarn "9"

module internal FontResolver =
    
    type FontTableEntry<'a> =
        {
            Tag             : 'a
            FamilyName      : string
            Offset          : int
            Weight          : int
            Italic          : bool
            SubFamilyName   : string
        }
        
    module FontTableEntries =
        // resolve according to: https://developer.mozilla.org/en-US/docs/Web/CSS/font-weight
        let chooseBestEntry (weight : int) (italic : bool) (available : list<FontTableEntry<'a>>) =

            let members =
                if italic then
                    let italics = available |> List.filter (fun m -> m.Italic)
                    match italics with
                    | [] -> available
                    | _ -> italics
                else
                    let nonitalic = available |> List.filter (fun m -> not m.Italic)
                    match nonitalic with
                    | [] -> available
                    | _ -> nonitalic
            
            let bestEntry =
                let map = members |> List.map (fun m -> m.Weight, m) |> MapExt.ofList
                let (l, s, r) = MapExt.neighbours weight map
                
                match s with
                | Some (_, m) -> m
                | None ->
                    // If the target weight given is between 400 and 500 inclusive:
                    if weight >= 400 && weight <= 500 then
                        // Look for available weights between the target and 500, in ascending order.
                        match r with
                        | Some (rw, rm) when rw <= 500 ->
                            rm
                        | _ ->
                            // If no match is found, look for available weights less than the target, in descending order.
                            match l with
                            | Some (lw, lm) ->
                                lm
                            | None ->
                                // If no match is found, look for available weights greater than 500, in ascending order.
                                Option.get r |> snd
                              
                    elif weight < 400 then
                        // If a weight less than 400 is given, look for available weights less than the target, in descending order.
                        // If no match is found, look for available weights greater than the target, in ascending order.
                        match l with
                        | Some (_, lm) -> lm
                        | None -> Option.get r |> snd
                            
                    else
                        // If a weight greater than 500 is given, look for available weights greater than the target, in ascending order.
                        // If no match is found, look for available weights less than the target, in descending order.
                        match r with
                        | Some (_, rm) -> rm
                        | None -> Option.get l |> snd
                
            bestEntry

        let ofStream (tag : 'a) (openStream : unit -> #Stream) =
            try
                let ofInfo (info : PreviewFontInfo) =
                    {
                        Tag = tag
                        FamilyName = info.TypographicFamilyName
                        Offset = info.ActualStreamOffset
                        Weight = int info.Weight
                        Italic = info.OS2TranslatedStyle.HasFlag Extensions.TranslatedOS2FontStyle.ITALIC || info.OS2TranslatedStyle.HasFlag Extensions.TranslatedOS2FontStyle.OBLIQUE
                        SubFamilyName = info.SubFamilyName 
                    }
                
                let r = OpenFontReader()
                use s = openStream() :> System.IO.Stream
                let info = r.ReadPreview s
                if info.IsFontCollection then
                    List.init info.MemberCount (info.GetMember >> ofInfo)
                else
                    [ofInfo info]
            with _ ->
                []
        
        let ofFile (file : string) =
            if System.IO.File.Exists file then
                ofStream file (fun () -> System.IO.File.OpenRead file)
            else
                []
        
        let read (openStream : 'a -> #System.IO.Stream) (entry : FontTableEntry<'a>) =
            let reader = OpenFontReader()
            use s = openStream entry.Tag :> System.IO.Stream
            reader.Read(s, entry.Offset, ReadFlags.Full)
        
        
    type FontTable<'a> (entries : seq<FontTableEntry<'a>>) =
        
        static let normalizeFamilyName (name : string) =
            name.ToLowerInvariant().Trim()
        
        
        
        let table =
            let dict = System.Collections.Generic.Dictionary<string, list<_>>()
            
            for e in entries do
                if not (isNull e.FamilyName) then
                    let key = normalizeFamilyName e.FamilyName
                
                    match dict.TryGetValue key with
                    | (true,  s) ->
                        dict.[key] <-  e :: s
                    | _ ->
                        dict.[key] <- [e]
                    
            dict
            
        let keys =
            table
            |> Seq.collect (fun (KeyValue(key, e)) -> e |> Seq.map (fun e -> e.FamilyName, key))
            |> Seq.toArray
        
        let names =
            keys |> Array.map fst
        
        
        member x.Find(family : string, weight : int, italic : bool) =
            let res = FuzzySharp.Process.ExtractOne(family, names)
            let (_, key) = keys.[res.Index]
            let entries = table.[key]
            FontTableEntries.chooseBestEntry weight italic entries
    
    module private Win32 =
        open System.Runtime.InteropServices

        type HKey =
            | HKEY_CLASSES_ROOT = 0x80000000
            | HKEY_CURRENT_USER = 0x80000001
            | HKEY_LOCAL_MACHINE = 0x80000002
            | HKEY_USERS = 0x80000003
            | HKEY_PERFORMANCE_DATA = 0x80000004
            | HKEY_CURRENT_CONFIG = 0x80000005
            | HKEY_DYN_DATA = 0x80000006

        type Flags =
            | RRF_RT_ANY = 0x0000ffff
            | RRF_RT_DWORD = 0x00000018
            | RRF_RT_QWORD = 0x00000048
            | RRF_RT_REG_BINARY = 0x00000008
            | RRF_RT_REG_DWORD = 0x00000010
            | RRF_RT_REG_EXPAND_SZ = 0x00000004
            | RRF_RT_REG_MULTI_SZ = 0x00000020
            | RRF_RT_REG_NONE = 0x00000001
            | RRF_RT_REG_QWORD = 0x00000040
            | RRF_RT_REG_SZ = 0x00000002

        [<DllImport("kernel32.dll")>]
        extern int RegGetValue(HKey hkey, string lpSubKey, string lpValue, Flags dwFlags, uint32& pdwType, nativeint pvData, uint32& pcbData)


        [<DllImport("kernel32.dll")>]
        extern int RegEnumValue(HKey hKey, int index, byte* lpValueName, int& lpcchValueName, void* lpReserved, void* lpType, byte* lpData, int& lpcbData)

        [<DllImport("kernel32.dll")>]
        extern int RegOpenKeyExA(HKey hHey, string lpSubKey, int ulOptions, int samDesired, HKey& res)

        let table =
            lazy (

                let mutable key = Unchecked.defaultof<HKey>
                let ret = RegOpenKeyExA(HKey.HKEY_LOCAL_MACHINE, "software\\microsoft\\windows nt\\currentversion\\Fonts", 0, 0x20019, &key)


                let mutable index = 0

                let mutable run = true

                let nameRx = System.Text.RegularExpressions.Regex(@"^(.*?)[ \t]*(Bold Italic|Light Italic|Thin Italic|Bold|Semibold|Thin|Italic|Light)?[ \t]*\([ \t]*(TrueType|OpenType)[ \t]*\)[ \t]*$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                
                let result = System.Collections.Generic.List<_>()
                let fonts = Environment.GetFolderPath Environment.SpecialFolder.Fonts
                while run do

                    let valueBuffer = Array.zeroCreate<byte> 8192
                    let mutable valueLen = valueBuffer.Length

                    let nameBuffer = Array.zeroCreate<byte> 8192
                    let mutable nameLen = nameBuffer.Length

                    use pValue = fixed valueBuffer
                    use pName = fixed nameBuffer
                    
                    
                    
                    let ret = RegEnumValue(key, index, pName, &nameLen, 0n, 0n, pValue, &valueLen)
                    if ret = 0 then
                        let name = System.Text.Encoding.ASCII.GetString(nameBuffer, 0, nameLen).Trim(' ', char 0)
                        let file = System.Text.Encoding.ASCII.GetString(valueBuffer, 0, valueLen).Trim(' ', char 0)
                        
                        let familyName =
                            let m = nameRx.Match name
                            if m.Success then
                                let value = m.Groups.[1].Value
                                if value.Contains ";" then None
                                else Some value
                            else
                                None
                        
                        let path = Path.Combine(fonts, file)
                        if File.Exists path then
                            let entries = FontTableEntries.ofFile path
                            match familyName with
                            | Some f when not (isNull f) ->
                                for r in entries do result.Add { r with FamilyName = f }
                            | _ ->
                                result.AddRange entries
                            
                                
                        index <- index + 1
                    else
                        run <- false
                        
                FontTable result
            )

    module private MacOs =
        open System.Runtime.InteropServices
        open System.IO
        open System.Linq
        open Typography
        open Typography.OpenFont
        open Aardvark.Base

        type CFArrayCallbackDelegate = delegate of nativeint * nativeint -> unit

        module CFText =
            [<Literal>]
            let private CoreFoundation = "/System/Library/Frameworks/CoreFoundation.framework/Versions/A/CoreFoundation"

            [<Literal>]
            let private CoreGraphics = "/System/Library/Frameworks/CoreGraphics.framework/Versions/A/CoreGraphics"
            
            [<Literal>]
            let private CoreText = "/System/Library/Frameworks/CoreText.framework/Versions/A/CoreText"
            
            
            [<Struct; StructLayout(LayoutKind.Sequential)>]
            type CFRange = { Start : nativeint; Length : nativeint }
            
            [<DllImport(CoreFoundation)>]
            extern void* CFDictionaryCreateMutable (void* a, int cap, void* b, void* c)

            [<DllImport(CoreText)>]
            extern void* CTFontCollectionCreateFromAvailableFonts (void* dict)

            [<DllImport(CoreText)>]
            extern void* CTFontCollectionCreateMatchingFontDescriptors(void* coll)

            [<DllImport(CoreFoundation)>]
            extern int CFArrayGetCount(void* arr)

            [<DllImport(CoreFoundation)>]
            extern void CFArrayApplyFunction(void* arr, CFRange range, CFArrayCallbackDelegate func, void* ctx)

            [<DllImport(CoreText)>]
            extern void* CTFontDescriptorCopyAttribute(void* desc, void* key)


            [<DllImport(CoreFoundation)>]
            extern void* CFStringCreateWithCString(void* a, string b, void* c)

            [<DllImport(CoreFoundation)>]
            extern void CFStringGetCString(void* str, byte* buffer, int len, void* encoding)

            [<DllImport(CoreFoundation)>]
            extern void* CFURLCopyFileSystemPath(void* url, int pathStyle)


            let table =
                lazy (

                    //let NSFontNameAttribute = CFStringCreateWithCString(0n, "NSFontNameAttribute", 0n)
                    let NSFontFamilyAttribute = CFStringCreateWithCString(0n, "NSFontFamilyAttribute", 0n)
                    let NSFontFaceAttribute = CFStringCreateWithCString(0n, "NSFontFaceAttribute", 0n)
                    let NSCTFontFileURLAttribute = CFStringCreateWithCString(0n, "NSCTFontFileURLAttribute", 0n)
                    
                    
                            
                    let ptr = CFDictionaryCreateMutable (0n, 100000, 0n, 0n)
                    
                    
                    let coll = CTFontCollectionCreateFromAvailableFonts ptr
                    let arr = CTFontCollectionCreateMatchingFontDescriptors coll
                    let cnt = CFArrayGetCount arr
                    
                    let mutable range = { Start = 0n; Length = nativeint cnt }
                    
                    let getString (font : nativeint) (att : nativeint) =
                        let test = CTFontDescriptorCopyAttribute(font, att)
                        let buffer = Array.zeroCreate<byte> 8192
                        use ptr = fixed buffer
                        CFStringGetCString(test, ptr, 8192, 0n)
                        
                        
                        let mutable l = 0
                        while l < buffer.Length && buffer.[l] <> 0uy do l <- l + 1
                        
                        
                        
                        System.Text.Encoding.UTF8.GetString(buffer,0, l)
                    
                    let getPath (font : nativeint) (att : nativeint) =
                        let test = CTFontDescriptorCopyAttribute(font, att)
                        let path = CFURLCopyFileSystemPath(test, 0)
                        
                        let buffer = Array.zeroCreate<byte> 8192
                        use ptr = fixed buffer
                        CFStringGetCString(path, ptr, 8192, 0n)
                        
                        let mutable l = 0
                        while l < buffer.Length && buffer.[l] <> 0uy do l <- l + 1
                        
                        
                        System.Text.Encoding.UTF8.GetString(buffer, 0, l)
                        
                    let files = System.Collections.Generic.Dictionary<string, System.Collections.Generic.HashSet<string>>()
                    let func =
                        CFArrayCallbackDelegate(fun ptr _ ->
                            let face = getString ptr NSFontFaceAttribute
                            let family = getString ptr NSFontFamilyAttribute
                            let path = getPath ptr NSCTFontFileURLAttribute
                            
                            match files.TryGetValue family with
                            | (true, set) -> set.Add path |> ignore
                            | _ ->
                                let set = System.Collections.Generic.HashSet<string>()
                                set.Add path |> ignore
                                files.[family] <- set
                           
                            
                        )
                    CFArrayApplyFunction(arr, range, func, 0n)
                    
                    
                    
                    let allEntries = System.Collections.Generic.List()
                    for KeyValue(family, files) in files do
                        for f in files do
                            
                            let entries = FontTableEntries.ofFile f |> List.map (fun i -> { i with FamilyName = family })
                            allEntries.AddRange entries
                            
                        
                    FontTable allEntries
                )
          
            
    
    
    let tryLoadTypeFace (family : string) (weight : int) (italic : bool) : Option<Typography.OpenFont.Typeface * string * int * bool> =
        try
            let entry = 
                match Environment.OSVersion with
                | Windows ->
                    Win32.table.Value.Find(family, weight, italic) |> Some
                | Mac ->
                    MacOs.CFText.table.Value.Find(family, weight, italic) |> Some
                | _ ->
                    failwith "not implemented"
            match entry with
            | Some entry ->
                let face = entry |> FontTableEntries.read File.OpenRead
                Some (face, entry.FamilyName, entry.Weight, entry.Italic)
            | None ->
                None
        with _ ->
            None

    let loadTypeface (family : string) (weight : int) (italic : bool) =
        match tryLoadTypeFace family weight italic with
        | Some file -> file
        | None -> failwithf "[Text] could not get font %s %A %s" family weight (if italic then "italic" else "")



