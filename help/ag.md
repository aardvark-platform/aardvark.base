# Attribute Grammar SceneGraph

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

## Attribute grammars to the rescue

Let's look at the OOP implementation again and elaborate its underlying problem. Suppose you want to add a function computing the
number of values in the tree (a simple count) and the list-types are defined in some place you don't have access to (maybe a 3rd-party library). <br />
The fundamental problem is that you would actually need to **extend** the interface and all concrete types with a new method.

This requirement sounds a lot like extension-methods but in fact it's a much harder problem since the
functions cannot be resolved at compile-time but instead should follow typical overloading/inheritance rules.

Our attribute grammar basically allows you to define something like extension-methods and provides dynamic
dispatch on these. Here's a little example defining an attribute **Count**

	// all attribute functions must be placed in a 
	// semantic type so they can be found efficiently.
	[<Semantic>]
	type Sem() =
		// Note that the definition looks nearly identical
		// to the member in the OOP example
		member x.Count(n : Node) =
			n.Left?Count() + 1 + n.Right?Count() 

		member x.Count(l : Leaf) =
			1

		// semantic functions can also be implemented 
		// for interfaces/base-classes. The Ag-system always 
		// looks for the most specific overload given a concrete type.
		// Therefore this function represents a default implementation.
		member x.Count(t : ITree) =
			0

Attributes can be queried using the **?** operator in F#
	
	let getCount (t : ITree) : int = t?Count()

There are two fundamental things to note here:

* The system cannot statically infer the attribute's type and we must therefore annotate it.
* Attribute lookups work on all types (to be specific they work on *Object*) and the system cannot guarantee that the lookup will succeed.

We decided to accept these caveats as they are necessary for the attribute grammar to be that extensible.

A common workaround (e.g. used by the SceneGraph implementation) is to add extension-methods for types which are known to define a specific attribute like
	
	// The attribute Count is defined for all ITrees
	// and its type is int
	type ITree with
		member x.Count() : int = x?Count()

	let test() =
		let myTree = Node(Leaf(1), 2, Leaf(3))
		let cnt = myTree.Count()


##States and Extensibility

Since we now know how we can compute the Sum, Length, etc. for datastructures we will extend the system to SceneGraphs.
A thing we haven't talked about yet is state.

Typically state is modelled explicitly
	
	type TraversalState = { shader : Shader; trafo : Trafo3d; ... }

and all functions on the SceneGraph take this TraversalState as an argument. Since it is not possible to list all kinds of
states here the state is typically implemented using HashTables and names (like in Aardvark 2010).
Furthermore one has to define how states propagate through the tree (e.g. trafos are changed by TrafoApplicators and are multiplied, etc.)

Attribute Grammars however are capable of expressing these things in a very clean way using so called **inherited attributes**.
All attributes defined so far are called **synthesized attributes**.

So let's define a simple inherited attribute and define its semantics

	[<Semanic>]
	type TrafoSem() =
		// the ag defines an artificial root-node which
		// is virtually placed on top of all things.
		member x.ModelTrafo(r : Root) =
			// for all children of Root the ModelTrafo shall be identity
			r.Child?ModelTrafo <- Trafo3d.Identity

		// TrafoApplicators change the trafo accordingly
		member x.ModelTrafo(t : TrafoApplicator) =
			// note the missing "()" in the query here which
			// denotes that the attribute is inherited
			r.Child?ModelTrafo <- t.Trafo * t?ModelTrafo

		// a simple synthesized attribute using the inherited one.
		member x.BoundingBox(l : GeometryLeaf) =
			let trafo : Trafo3d = l?ModelTrafo
			l.Box.Transformed(trafo)

Note that inherited attriubtes are only valid "inside" the tree which means that the (mostly) do not have a meaning outside the attriubte system.
(e.g. outside the system a shader-attribute does not have a proper value). <br />
This leads to the conclusion that only synthesized attributes can be "pulled out" of a datastructure but they may internally use inherited attributes for managing state.

A thing to note is that inherited attriubtes are automatically *passed through* nodes which are not associated with any rule for the attribute.


##SceneGraph

In the following I'll give a short overview of the SceneGraph (as defined in [Aardvark.Rendering](http://burrow.ra1.vrvis.lan/vrvis/aardvark-rendering/tree/master)).

###Concept
The SceneGraph (Sg) is mainly designed to produce **RenderJobs** which represent draw-calls with all needed arguments. <br />
Since the Sg might be dynamic this set of RenderJobs may change as the Sg is modified. <br />
Therefore the Sg uses incremental datastructures (see [????](????)) for its own structure and all attributes.

All Sg nodes implement the interface **ISg** which just serves as marker for all of the attribute-extensions, etc. <br />
Another important class defined is **AbstractApplicator** which defines a constructor taking a single child-graph. 
This type is very useful when extending the Sg since all synthesized attributes are (by default) passed through it.

###Nodes

Here's a list of all types currently defined in the Sg with their primary contstructors indicating their respective intent
	

	// the only leaf-node in the Sg
	type RenderNode(call : IMod<DrawCallInfo>)
	
	// Vertex-/InstanceAttriubtes and VertexIndices
	type VertexAttributeApplicator(values : Map<Symbol, BufferView>, child : IMod<ISg>)
	type VertexIndexApplicator(value : IMod<Array>, child : IMod<ISg>)
	type InstanceAttributeApplicator(values : Map<Symbol, BufferView>, child : IMod<ISg>)
	
	
	// Uniforms/Surfaces/Trafos
	type UniformApplicator(uniformHolder : IUniformProvider, child : IMod<ISg>)
	type SurfaceApplicator(surface : IMod<ISurface>, child : IMod<ISg>)
	type TextureApplicator(semantic : Symbol, texture : IMod<ITexture>, child : IMod<ISg>)
	type TrafoApplicator(trafo : IMod<Trafo3d>, child : IMod<ISg>)
	type ViewTrafoApplicator(trafo : IMod<Trafo3d>, child : IMod<ISg>)
	type ProjectionTrafoApplicator(trafo : IMod<Trafo3d>, child : IMod<ISg>)

	// Render Modes
	type DepthTestModeApplicator(mode : IMod<DepthTestMode>, child : IMod<ISg>)
	type CullModeApplicator(mode : IMod<CullMode>, child : IMod<ISg>)
	type FillModeApplicator(mode : IMod<FillMode>, child : IMod<ISg>)
	type StencilModeApplicator(mode : IMod<StencilMode>, child : IMod<ISg>)
	type BlendModeApplicator(mode : IMod<BlendMode>, child : IMod<ISg>)

	// Grouping
	type Group(elements : seq<ISg>)
	type Set(content : aset<ISg>)
	
	// Special Nodes
	type AdapterNode(node : obj)
	type DynamicNode(child : IMod<ISg>)
	type OnOffNode(on : IMod<bool>, child : IMod<ISg>)
	type PassApplicator(pass : IMod<uint64>, child : IMod<ISg>)


###Default Semantics

Some synthesized attributes are defined for all Nodes:
	
	type ISg with
		member x.RenderJobs() : aset<RenderJob>
		member x.GlobalBoundingBox() : IMod<Box3d>
		member x.LocalBoundingBox() : IMod<Box3d>

And a lot more inherited attriubtes:

	type ISg with
		// Trafos
		member x.ModelTrafo : IMod<Trafo3d>
		member x.ViewTrafo : IMod<Trafo3d>
		member x.ProjTrafo : IMod<Trafo3d>
		
		// Uniforms
		member x.Uniforms : list<IUniformProvider>
		member x.HasDiffuseColorTexture : IMod<bool>
		member x.DiffuseColorTexture : IMod<ITexture>
		//...

		// Surface and modes
		member x.Surface : IMod<ISurface>
		member x.FillMode : IMod<FillMode>
		//...

		// Vertex-/InstanceAttriubtes
		member x.VertexAttributes : Map<Symbol, BufferView> 
		member x.InstanceAttributes : Map<Symbol, BufferView> 
		member x.VertexIndexArray : IMod<Array>

An important thing to note is that the backend searches for uniforms in the following order

* Search the IUniformHolders given in the attribute Uniforms
* Search the set of uniforms defined in the surface itself (if any)
* Search for an Attriubte having the desired name

Vertex attributes etc. are queried in a similar way.


##Further Reading

* [Why Attriubte Grammars Matter](https://wiki.haskell.org/The_Monad.Reader/Issue4/Why_Attribute_Grammars_Matter)
* [Harald's Master's Thesis](https://github.com/haraldsteinlechner/research/blob/master/papers/AttributeGrammarsForSceneGraphs.pdf)