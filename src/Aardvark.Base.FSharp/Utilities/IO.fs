namespace Aardvark.Base

open System.IO

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module IO =
    let alterFileName (str: string) (f: string -> string) = Path.Combine (Path.GetDirectoryName str, f (Path.GetFileName str))

    let createFileStream path =
        if File.Exists path
        then File.Delete path
        new FileStream(path, FileMode.CreateNew)


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Path =

    /// Combines a sequence of strings into a path.
    let combine (paths: seq<string>) =
        Path.Combine(paths |> Seq.toArray)

    /// Combines two strings into a path.
    let andPath (first: string) (second: string) =
        Path.Combine(first, second)

    /// Normalizes the directory separators of the given path, i.e. replaces
    /// the alternative separator with the default separator.
    let normalizeDirectorySeparators (path: string) =
        path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar)

    /// Adds a trailing slash unless the path already has one.
    let withTrailingSlash (path: string) =
#if NET6_0_OR_GREATER
        if path.EndsWith Path.DirectorySeparatorChar || path.EndsWith Path.AltDirectorySeparatorChar then
#else
        if path.EndsWith(string Path.DirectorySeparatorChar) || path.EndsWith(string Path.AltDirectorySeparatorChar) then
#endif
            path
        else
            path + string Path.DirectorySeparatorChar

    /// Removes the trailing slash from the given path if it has one.
    let withoutTrailingSlash (path: string) =
#if NET6_0_OR_GREATER
        if path.EndsWith Path.DirectorySeparatorChar || path.EndsWith Path.AltDirectorySeparatorChar then
#else
        if path.EndsWith(string Path.DirectorySeparatorChar) || path.EndsWith(string Path.AltDirectorySeparatorChar)  then
#endif
            path.Substring(0, path.Length - 1)
        else
            path

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Stream =

    /// <summary>
    /// Reads the contents of a stream into a byte array.
    /// </summary>
    /// <param name="stream">The stream to read.</param>
    /// <returns>A byte array containing the contents of the stream.</returns>
    let readAllBytes (stream: Stream) =
        if stream.CanSeek then
            let buffer = Array.zeroCreate<byte> <| int stream.Length
            use ms = new MemoryStream(buffer)
            stream.CopyTo ms
            buffer
        else
            use ms = new MemoryStream()
            stream.CopyTo ms
            ms.ToArray()

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module File =

    /// <summary>
    /// Creates the parent directory of the given file path, if it does not exist.
    /// </summary>
    /// <param name="path">The path of the file, whose parent directory is to be created.</param>
    let createParentDirectory (path : string) =
        let info = FileInfo(path)
        if not info.Directory.Exists then
            info.Directory.Create()

    /// <summary>
    /// Creates a new file, writes the specified string array to the file, and then closes the file.
    /// </summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="lines">The lines to write to the file.</param>
    let writeAllLines (path : string) (lines : string[]) =
        File.WriteAllLines(path, lines)

    /// <summary>
    /// Creates a new file, writes the specified string array to the file, and then closes the file.
    /// If the parent directory does not exist, it is created first.
    /// </summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="lines">The lines to write to the file.</param>
    let writeAllLinesSafe (path : string) (lines : string[]) =
        createParentDirectory path
        File.WriteAllLines(path, lines)

    /// <summary>
    /// Creates a new file, writes the specified string to the file, and then closes the file.
    /// </summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="text">The string to write to the file.</param>
    let writeAllText (path : string) (text : string) =
        File.WriteAllText(path, text)

    /// <summary>
    /// Creates a new file, writes the specified string to the file, and then closes the file.
    /// If the parent directory does not exist, it is created first.
    /// </summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="text">The string to write to the file.</param>
    let writeAllTextSafe (path : string) (text : string) =
        createParentDirectory path
        File.WriteAllText(path, text)

    /// <summary>
    /// Creates a new file, writes the specified byte array to the file, and then closes the file.
    /// </summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="bytes">The bytes to write to the file.</param>
    let writeAllBytes (path : string) (bytes : uint8[]) =
        File.WriteAllBytes(path, bytes)

    /// <summary>
    /// Creates a new file, writes the specified byte array to the file, and then closes the file.
    /// If the parent directory does not exist, it is created first.
    /// </summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="bytes">The bytes to write to the file.</param>
    let writeAllBytesSafe (path : string) (bytes : uint8[]) =
        createParentDirectory path
        File.WriteAllBytes(path, bytes)

    /// <summary>
    /// Opens a text file, reads all lines of the file into a string array, and then closes the file.
    /// </summary>
    /// <param name="path">The file to open for reading.</param>
    /// <returns>A string array containing all lines of the file.</returns>
    let readAllLines (path : string) =
        File.ReadAllLines path

    /// <summary>
    /// Opens a text file, reads all the text in the file into a string, and then closes the file.
    /// </summary>
    /// <param name="path">The file to open for reading.</param>
    /// <returns>A string containing all the text in the file.</returns>
    let readAllText (path : string) =
        File.ReadAllText path

    /// <summary>
    /// Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
    /// </summary>
    /// <param name="path">The file to open for reading.</param>
    /// <returns>A byte array containing the contents of the file.</returns>
    let readAllBytes (path : string) =
        File.ReadAllBytes path