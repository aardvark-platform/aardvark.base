# Aardvark Base Tutorials

This page conains basic tutorials illustrating the functionality provided
by *Aardvark.Base*, *Aardvark.Base.FSharp* and *Aardvark.Base.Incremental*.

## Tutorials
* [Attribute Grammar](ag.html)

This page shall contain examples for all aardvark-related things.

### Vector Stuff
    #r @"..\bin\Release\Aardvark.Base.dll"
    open Aardvark.Base
    
    let someThing() =
        V2i(1,1) + V2.II * 10
        
### Other Stuff

    open Aardvark.Base
    
    let someThing() =
        V2i(1,1) + V2.II * 10
        
* [Simple Example](test.html)