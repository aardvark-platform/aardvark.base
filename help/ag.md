# Attribute Grammar Tutorial

Attribute Grammars represent a formal way of defining attributes on recursive data structures (such as lists, trees, etc.).

Suppose you need to compute the sum of all values in a tree like:
    
    type ITree = interface end
    
    type Node(l : ITree, value : int, r : ITree) =
        interface ITree
        member x.Left = l
        member x.Right = r
        member x.Value = value
    
    type Leaf(value : int) =
        interface ITree
        member x.Value = value


You could obviously write a simple function like:
    
    let rec sum (t : ITree) =
        match t with
            | :? Node as n -> 
                sum n.Left + n.Value + n.Right
            | :? Leaf as n ->
                n.Value