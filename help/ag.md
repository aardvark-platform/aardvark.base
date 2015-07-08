# Attribute Grammar Tutorial

Attribute Grammars represent a formal way of defining attributes on recursive data structures (such as lists, trees, etc.).


## Problem
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

The standard OOP approach here is to extend the interface adding a method sum which needs to be implemented
by all types.

	type ITree =
		abstract member Sum : unit -> int
		
    type Node(l : ITree, value : int, r : ITree) =
        interface ITree with
			member x.Sum() = l.Sum() + value + r.Sum()
			//...
    
    type Leaf(value : int) =
        interface ITree with
			member x.Sum() = value
			//...

Whenever a new functionality is needed the interface and all implementations need to be
changed appropriately.
			
**Therefore the OOP approach is not extensible in terms of functionality.**

The functional approach would be to define a function sum which performs the type-dispatch directly (in contrast to inheritance).

    let rec sum (t : ITree) =
        match t with
            | :? Node as n -> 
                sum n.Left + n.Value + n.Right
            | :? Leaf as n ->
                n.Value
			// needs to be extended for additional types
				
As the example illustrates every functionality must include a list of all valid types.
Whenever a new type is added all existing functions need to be changed (including a case for the new type).

**Therefore the functional approach is not extensible in terms of types.**

This is commonly known as the *Expression Problem*

## Solution

Let's look at the OOP implementation again and elaborate its underlying problem. Suppose you want to add a function computing the
number of values in the tree (a simple count) and the list-types are defined in some place you don't have access to (maybe a 3rd-party library). <br />
The fundamental problem is that you would actually need to **extend** the interface and all concrete types with a new method.

This requirement sounds a lot like extension-methods but in fact it's a much harder problem since the
functions cannot be resolved at compile-time but instead should follow typical overloading/inheritance rules.

Our attribute grammar basically allows you to define something like extension-methods and provides dynamic
dispatch on these. Here's and illustrative example

	// all attribute functions must be placed in a 
	// semantic type so they can be found efficiently.
	[<Semantic>]
	type Sem() =
		// Note that the definition looks nearly identical
		// to the member in the OOP example
		member x.Sum(n : Node) =
			n.Left?Sum() + n.Value + n.Right?Sum() 
