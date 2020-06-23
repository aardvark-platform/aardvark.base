open System
open System.IO
open System.Text
open System.Text.RegularExpressions

let qrDecompose = Regex @"([ \t]*)//[ \t]*__MATRIX_QR_DECOMPOSE__[ \t]*"
let rqDecompose = Regex @"([ \t]*)//[ \t]*__MATRIX_RQ_DECOMPOSE__[ \t]*"
let qrBidiagonalize = Regex @"([ \t]*)//[ \t]*__MATRIX_QR_BIDIAGONALIZE__[ \t]*"
let svdDecompose = Regex @"([ \t]*)//[ \t]*__MATRIX_SVD_DECOMPOSE__[ \t]*"


let svdFile = Path.Combine(__SOURCE_DIRECTORY__, "SVD.fs")
let qrFile = Path.Combine(__SOURCE_DIRECTORY__, "QR.fs")

let qrDecomposeCode (indent : string) =
    let b = StringBuilder()
    let printfn fmt =fmt |> Printf.kprintf (fun str -> b.Append indent |> ignore; b.AppendLine str |> ignore) 
    for (c, t) in ["f", "float32"; "d", "float"] do
        for rows in 2 .. 4 do
            for cols in rows .. min 4 (rows + 1) do
                let mat = sprintf "M%d%d%s" rows cols c
                let qmat = sprintf "M%d%d%s" rows rows c

                printfn "[<MethodImpl(MethodImplOptions.NoInlining)>]"
                printfn "static member DecomposeV(m : %s) = " mat
                printfn "    let pQ = NativePtr.stackalloc<%s> 1" qmat
                printfn "    let pR = NativePtr.stackalloc<%s> 1" mat
                printfn "    NativePtr.write pR m"
                printfn "    let tQ = NativeMatrix<%s>(NativePtr.cast pQ, MatrixInfo(0L,V2l(%d,%d),V2l(1,%d)))" t rows rows rows
                printfn "    let tR = NativeMatrix<%s>(NativePtr.cast pR, MatrixInfo(0L,V2l(%d,%d),V2l(1,%d)))" t cols rows cols
                printfn "    QR.DecomposeInPlace(tQ, tR)"
                printfn "    struct(NativePtr.read pQ, NativePtr.read pR)"
                printfn ""
                printfn "static member Decompose(m : %s) = " mat
                printfn "    let struct(q, r) = QR.DecomposeV(m)"
                printfn "    (q,r)"
                printfn ""
    b.ToString()

let qrBidiagonalizeCode (indent : string) =
    let b = StringBuilder()
    let printfn fmt =fmt |> Printf.kprintf (fun str -> b.Append indent |> ignore; b.AppendLine str |> ignore) 
    for (c, t) in ["f", "float32"; "d", "float"] do
        for rows in 2 .. 4 do
            for cols in rows .. min 4 (rows + 1) do
                let mat = sprintf "M%d%d%s" rows cols c
                let lmat = sprintf "M%d%d%s" rows rows c
                let rmat = sprintf "M%d%d%s" cols cols c
            
                printfn "[<MethodImpl(MethodImplOptions.NoInlining)>]"
                printfn "static member BidiagonalizeV(m : %s) = " mat
                printfn "    let pU = NativePtr.stackalloc<%s> 1" lmat
                printfn "    let pB = NativePtr.stackalloc<%s> 1" mat
                printfn "    let pV = NativePtr.stackalloc<%s> 1" rmat
                printfn "    NativePtr.write pB m"
                printfn "    let tU = NativeMatrix<%s>(NativePtr.cast pU, MatrixInfo(0L, V2l(%d,%d), V2l(1, %d)))" t rows rows rows
                printfn "    let tB = NativeMatrix<%s>(NativePtr.cast pB, MatrixInfo(0L, V2l(%d,%d), V2l(1, %d)))" t cols rows cols
                printfn "    let tV = NativeMatrix<%s>(NativePtr.cast pV, MatrixInfo(0L, V2l(%d,%d), V2l(1, %d)))" t cols cols cols
                printfn "    QR.BidiagonalizeInPlace(tU,tB,tV)"
                printfn "    struct(NativePtr.read pU, NativePtr.read pB, NativePtr.read pV)"
                printfn ""
                printfn "static member Bidiagonalize(m : %s) = " mat
                printfn "    let struct(u, b, vt) = QR.BidiagonalizeV(m)"
                printfn "    (u, b, vt)"
                printfn ""
    b.ToString()

let rqDecomposeCode (indent : string) =
    let b = StringBuilder()
    let printfn fmt =fmt |> Printf.kprintf (fun str -> b.Append indent |> ignore; b.AppendLine str |> ignore) 
    for (c, t) in ["f", "float32"; "d", "float"] do
        for rows in 2 .. 4 do
            for cols in rows .. min 4 (rows + 1) do
                let mat = sprintf "M%d%d%s" rows cols c
                let qmat = sprintf "M%d%d%s" cols cols c

                printfn "[<MethodImpl(MethodImplOptions.NoInlining)>]"
                printfn "static member DecomposeV(m : %s) = " mat
                printfn "    let pR = NativePtr.stackalloc<%s> 1" mat
                printfn "    let pQ = NativePtr.stackalloc<%s> 1" qmat
                printfn "    NativePtr.write pR m"
                printfn "    let tR = NativeMatrix<%s>(NativePtr.cast pR, MatrixInfo(0L,V2l(%d,%d),V2l(1,%d)))" t cols rows cols
                printfn "    let tQ = NativeMatrix<%s>(NativePtr.cast pQ, MatrixInfo(0L,V2l(%d,%d),V2l(1,%d)))" t cols cols cols
                printfn "    RQ.DecomposeInPlace(tR, tQ)"
                printfn "    struct(NativePtr.read pR, NativePtr.read pQ)"
                printfn ""
                printfn "static member Decompose(m : %s) = " mat
                printfn "    let struct(r, q) = RQ.DecomposeV(m)"
                printfn "    (r, q)"
                printfn ""
    b.ToString()

let qrReplacements =
    [
        qrDecompose, qrDecomposeCode
        qrBidiagonalize, qrBidiagonalizeCode
        rqDecompose, rqDecomposeCode
    ]

do
    let mutable text = File.ReadAllText qrFile

    for (pattern, repl) in qrReplacements do
        let m0 = pattern.Match text
        if m0.Success then
            let m1 = pattern.Match(text, m0.Index + m0.Length)
            if m1.Success then
                let pre = text.Substring(0, m0.Index + m0.Length)
                let post = text.Substring(m1.Index)

                let indent = m0.Groups.[1].Value
                let repl = repl indent

                text <- pre + "\r\n" + repl + post

    File.WriteAllText(qrFile, text)


let svdDecomposeCode (indent : string) =
    let b = StringBuilder()
    let printfn fmt =fmt |> Printf.kprintf (fun str -> b.Append indent |> ignore; b.AppendLine str |> ignore) 
    for (c, t) in ["f", "float32"; "d", "float"] do
        for rows in 2 .. 4 do
            for cols in rows .. min 4 (rows + 1) do
                let mat = sprintf "M%d%d%s" rows cols c
                let lmat = sprintf "M%d%d%s" rows rows c
                let rmat = sprintf "M%d%d%s" cols cols c
            
                printfn "[<MethodImpl(MethodImplOptions.NoInlining)>]"
                printfn "static member DecomposeV(m : %s) = " mat
                printfn "    let pU  = NativePtr.stackalloc<%s> 1" lmat
                printfn "    let pS  = NativePtr.stackalloc<%s> 1" mat
                printfn "    let pVt = NativePtr.stackalloc<%s> 1" rmat
                printfn "    NativePtr.write pS m"
                printfn "    let tU  = NativeMatrix<%s>(NativePtr.cast pU,  MatrixInfo(0L, V2l(%d,%d), V2l(1, %d)))" t rows rows rows
                printfn "    let tS  = NativeMatrix<%s>(NativePtr.cast pS,  MatrixInfo(0L, V2l(%d,%d), V2l(1, %d)))" t cols rows cols
                printfn "    let tVt = NativeMatrix<%s>(NativePtr.cast pVt, MatrixInfo(0L, V2l(%d,%d), V2l(1, %d)))" t cols cols cols
                printfn "    if SVD.DecomposeInPlace(tU,tS,tVt) then"
                printfn "        ValueSome(struct(NativePtr.read pU, NativePtr.read pS, NativePtr.read pVt))"
                printfn "    else"
                printfn "        ValueNone"
                printfn ""
                printfn "static member Decompose(m : %s) = " mat
                printfn "    match SVD.DecomposeV(m) with"
                printfn "    | ValueSome(struct(u, s, vt)) -> Some(u, s, vt)"
                printfn "    | ValueNone -> None"
                printfn ""
    b.ToString()


let svdReplacements =
    [
        svdDecompose, svdDecomposeCode
    ]

do
    let mutable text = File.ReadAllText svdFile

    for (pattern, repl) in svdReplacements do
        let m0 = pattern.Match text
        if m0.Success then
            let m1 = pattern.Match(text, m0.Index + m0.Length)
            if m1.Success then
                let pre = text.Substring(0, m0.Index + m0.Length)
                let post = text.Substring(m1.Index)

                let indent = m0.Groups.[1].Value
                let repl = repl indent

                text <- pre + "\r\n" + repl + post

    File.WriteAllText(svdFile, text)





