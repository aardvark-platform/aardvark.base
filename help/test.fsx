(*** hide ***)
#r @"Aardvark.Base.dll"
open Aardvark.Base
    
(**
#Basic Examples
##A very simple function using V2i
*)
let vecusage() =
    V2i(1,1) + V2i.II * 10


(**
## printing values
*)
printfn "%A" 0
(** will print *)
(**
    (*** raw ***)
    [0,0]
*)
