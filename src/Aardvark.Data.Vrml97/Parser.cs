using Aardvark.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Aardvark.Data.Vrml97
{
    /// <summary>
    /// Symbol table.
    /// </summary>
    public static class Vrml97Sym
    {
#pragma warning disable 1591
        public static readonly Symbol Vrml97 = "Vrml97";
        public static readonly Symbol url = "url";
        public static readonly Symbol texture = "texture";
        public static readonly Symbol name = "name";
        public static readonly Symbol filename = "filename";
        public static readonly Symbol node = "node";
        public static readonly Symbol root = "root";
        public static readonly Symbol appearance = "appearance";
        public static readonly Symbol material = "material";
        public static readonly Symbol textureTransform = "textureTransform";
        public static readonly Symbol center = "center";
        public static readonly Symbol rotation = "rotation";
        public static readonly Symbol scale = "scale";
        public static readonly Symbol translation = "translation";
        public static readonly Symbol scaleOrientation = "scaleOrientation";

        public static readonly Symbol DEF = "DEF";
        public static readonly Symbol USE = "USE";
        public static readonly Symbol ROUTE = "ROUTE";
        public static readonly Symbol NULL = "NULL";
#pragma warning restore 1591
    }

    /// <summary>
    /// Table of VRML97 node names
    /// </summary>
    public static class Vrml97NodeName
    {
#pragma warning disable 1591
        public static readonly Symbol Anchor = "Anchor";
        public static readonly Symbol Appearance = "Appearance";
        public static readonly Symbol AudioClip = "AudioClip";
        public static readonly Symbol Background = "Background";
        public static readonly Symbol Billboard = "Billboard";
        public static readonly Symbol Box = "Box";
        public static readonly Symbol Collision = "Collision";
        public static readonly Symbol Color = "Color";
        public static readonly Symbol ColorInterpolator = "ColorInterpolator";
        public static readonly Symbol Cone = "Cone";
        public static readonly Symbol Coordinate = "Coordinate";
        public static readonly Symbol CoordinateInterpolator = "CoordinateInterpolator";
        public static readonly Symbol Cylinder = "Cylinder";
        public static readonly Symbol CylinderSensor = "CylinderSensor";
        public static readonly Symbol DirectionalLight = "DirectionalLight";
        public static readonly Symbol ElevationGrid = "DirectionalLight";
        public static readonly Symbol Extrusion = "DirectionalLight";
        public static readonly Symbol Fog = "Fog";
        public static readonly Symbol FontStyle = "FontStyle";
        public static readonly Symbol Group = "Group";
        public static readonly Symbol ImageTexture = "ImageTexture";
        public static readonly Symbol IndexedFaceSet = "IndexedFaceSet";
        public static readonly Symbol IndexedLineSet = "IndexedLineSet";
        public static readonly Symbol Inline = "Inline";
        public static readonly Symbol LOD = "LOD";
        public static readonly Symbol Material = "Material";
        public static readonly Symbol MovieTexture = "MovieTexture";
        public static readonly Symbol NavigationInfo = "NavigationInfo";
        public static readonly Symbol Normal = "Normal";
        public static readonly Symbol NormalInterpolator = "NormalInterpolator";
        public static readonly Symbol OrientationInterpolator = "OrientationInterpolator";
        public static readonly Symbol PixelTexture = "PixelTexture";
        public static readonly Symbol PlaneSensor = "PlaneSensor";
        public static readonly Symbol PointLight = "PointLight";
        public static readonly Symbol PointSet = "PointSet";
        public static readonly Symbol PositionInterpolator = "PositionInterpolator";
        public static readonly Symbol ProximitySensor = "ProximitySensor";
        public static readonly Symbol ScalarInterpolator = "ScalarInterpolator";
        public static readonly Symbol Script = "Script";
        public static readonly Symbol Shape = "Shape";
        public static readonly Symbol Sound = "Sound";
        public static readonly Symbol Sphere = "Sphere";
        public static readonly Symbol SphereSensor = "SphereSensor";
        public static readonly Symbol SpotLight = "SpotLight";
        public static readonly Symbol Switch = "Switch";
        public static readonly Symbol Text = "Text";
        public static readonly Symbol TextureCoordinate = "TextureCoordinate";
        public static readonly Symbol TextureTransform = "TextureTransform";
        public static readonly Symbol TimeSensor = "TimeSensor";
        public static readonly Symbol TouchSensor = "TTouchSensorext";
        public static readonly Symbol Transform = "Transform";
        public static readonly Symbol Viewpoint = "Viewpoint";
        public static readonly Symbol VisibilitySensor = "VisibilitySensor";
        public static readonly Symbol WorldInfo = "WorldInfo";
#pragma warning restore 1591
    }

    /// <summary>
    /// Vrml97 parser.
    /// Creates a parse tree from a file, or a stream reader.
    /// 
    /// Example:
    /// Parser parser = new Parser("myVrmlFile.wrl");
    /// SymMapBase parseTree = parser.Perform();
    /// 
    /// </summary>
    public class Parser : IDisposable
    {
        Stream m_inputStream;

        #region Public interface.

        /// <summary>
        /// Constructs a Parser for the given input stream.
        /// In order to actually parse the data, call the
        /// Perform method, which returns a SymMapBase containing
        /// the parse tree.
        /// </summary>
        /// <param name="input">Input stream.</param>
        /// <param name="fileName"></param>
        public Parser(Stream input, string fileName)
        {
            m_result.TypeName = Vrml97Sym.Vrml97;
            m_result[Vrml97Sym.filename] = fileName;
            m_inputStream = input;
            m_tokenizer = new Tokenizer(input);
        }

        /// <summary>
        /// Constructs a Parser for the given file.
        /// In order to actually parse the data, call the
        /// Perform method, which returns a SymMapBase
        /// containing the parse tree.
        /// This constructor creates an internal file stream and Dispose
        /// needs to be called when the Parser is no longer needed.
        /// </summary>
        /// <param name="fileName">Input filename.</param>
        public Parser(string fileName)
        {
            m_result.TypeName = Vrml97Sym.Vrml97;
            m_result[Vrml97Sym.filename] = fileName;

            m_inputStream = new FileStream(
                                    fileName,
                                    FileMode.Open, FileAccess.Read, FileShare.Read,
                                    4096, false
                                    );

            m_tokenizer = new Tokenizer(m_inputStream);
        }

        /// <summary>
        /// Parses the input data and returns a SymMapBase
        /// containing the parse tree.
        /// </summary>
        /// <returns>Parse tree.</returns>
        public Vrml97Scene Perform()
        {
            var root = new List<SymMapBase>();

            while (true)
            {
				try
				{
                    var node = ParseNode(m_tokenizer);
					if (node == null) break;
					root.Add(node);
					Thread.Sleep(0);
				}
				catch (ParseException e)
				{
					Console.WriteLine("WARNING: Caught exception while parsing: {0}!", e.Message);
                    Console.WriteLine("WARNING: Result may contain partial, incorrect or invalid data!");
					break;
				}
            }

            m_result[Vrml97Sym.root] = root;
            return new Vrml97Scene(m_result);
        }

        #endregion

        #region Node specs.

        public static readonly FieldParser SFBool = new FieldParser(ParseSFBool);
        public static readonly FieldParser MFBool = new FieldParser(ParseMFBool);
        public static readonly FieldParser SFColor = new FieldParser(ParseSFColor);
        public static readonly FieldParser MFColor = new FieldParser(ParseMFColor);
        public static readonly FieldParser SFFloat = new FieldParser(ParseSFFloat);
        public static readonly FieldParser MFFloat = new FieldParser(ParseMFFloat);
        public static readonly FieldParser SFImage = new FieldParser(ParseSFImage);
        public static readonly FieldParser SFInt32 = new FieldParser(ParseSFInt32);
        public static readonly FieldParser MFInt32 = new FieldParser(ParseMFInt32);
        public static readonly FieldParser SFNode = new FieldParser(ParseSFNode);
        public static readonly FieldParser MFNode = new FieldParser(ParseMFNode);
        public static readonly FieldParser SFRotation = new FieldParser(ParseSFRotation);
        public static readonly FieldParser MFRotation = new FieldParser(ParseMFRotation);
        public static readonly FieldParser SFString = new FieldParser(ParseSFString);
        public static readonly FieldParser MFString = new FieldParser(ParseMFString);
        public static readonly FieldParser SFTime = new FieldParser(ParseSFFloat);
        public static readonly FieldParser MFTime = new FieldParser(ParseMFFloat);
        public static readonly FieldParser SFVec2f = new FieldParser(ParseSFVec2f);
        public static readonly FieldParser MFVec2f = new FieldParser(ParseMFVec2f);
        public static readonly FieldParser SFVec3f = new FieldParser(ParseSFVec3f);
        public static readonly FieldParser MFVec3f = new FieldParser(ParseMFVec3f);

        /// <summary>
        /// Registers a custom node attribute field parser.
        /// Usage: RegisterCustomNodeField(Vrml97NodeName.Material, "doubleSided", Parser.SFBool, false);
        /// </summary>
        public static void RegisterCustomNodeField(Symbol nodeName, Symbol fieldName, FieldParser parser, object defaultValue)
        {
            if (!s_parseInfoMap.TryGetValue(nodeName, out var npi))
                throw new ArgumentException($"Failed to register \"{fieldName}\" as custom \"{nodeName}\" node field: The node name is not registered!");
            if (npi.FieldDefs == null)
                throw new ArgumentException($"Failed to register \"{fieldName}\" as custom \"{nodeName}\" node field: The node does not have FieldDefs!");
            npi.FieldDefs.Add(fieldName, (parser, defaultValue));
        }

        /** Static constructor. */
        static Parser()
        {
            // Lookup table for Vrml97 node types.
            // For each node type a NodeParseInfo entry specifies how
            // to handle this kind of node.
            s_parseInfoMap = new SymbolDict<NodeParseInfo>
            {
                // DEF
                [Vrml97Sym.DEF] = new NodeParseInfo(new NodeParser(ParseDEF)),

                // USE
                [Vrml97Sym.USE] = new NodeParseInfo(new NodeParser(ParseUSE)),

                // ROUTE
                [Vrml97Sym.ROUTE] = new NodeParseInfo(new NodeParser(ParseROUTE)),

                // NULL
                [Vrml97Sym.NULL] = new NodeParseInfo(new NodeParser(ParseNULL))
            };

            var defaultBBoxCenter = (SFVec3f, (object)V3f.Zero);
            var defaultBBoxSize = (SFVec3f, (object)new V3f(-1, -1, -1));

            (FieldParser, object) fd(FieldParser fp) => (fp, null); // helper to create (FieldParser, <defaultValue>) tuple with null as default value

            // Anchor
            s_parseInfoMap[Vrml97NodeName.Anchor] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "children", fd(MFNode) },
                    { "description", fd(SFString) },
                    { "parameter", fd(MFString) },
                    { "url", fd(MFString) },
                    { "bboxCenter", defaultBBoxCenter},
                    { "bboxSize", defaultBBoxSize}
                });

            // Appearance
            s_parseInfoMap[Vrml97NodeName.Appearance] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "material", fd(SFNode) },
                    { "texture", fd(SFNode) },
                    { "textureTransform", fd(SFNode) }
                });

            // AudioClip
            s_parseInfoMap[Vrml97NodeName.AudioClip] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "description", fd(SFString) },
                    { "loop", (SFBool, false) },
                    { "pitch", (SFFloat, 1.0f) },
                    { "startTime", (SFTime, 0.0f)},
                    { "stopTime", (SFTime, 0.0f)},
                    { "url", fd(MFString)}
                });

            // Background
            s_parseInfoMap[Vrml97NodeName.Background] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "groundAngle", fd(MFFloat) },
                    { "groundColor", fd(MFColor) },
                    { "backUrl", fd(MFString) },
                    { "bottomUrl", fd(MFString) },
                    { "frontUrl", fd(MFString) },
                    { "leftUrl", fd(MFString) },
                    { "rightUrl", fd(MFString) },
                    { "topUrl", fd(MFString) },
                    { "skyAngle", fd(MFFloat) },
                    { "skyColor", (MFColor, C3f.Black) }
                });

            // Billboard
            s_parseInfoMap[Vrml97NodeName.Billboard] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "axisOfRotation", (SFVec3f, new V3f(0.0f, 1.0f, 0.0f)) },
                    { "children", fd(MFNode) },
                    { "bboxCenter", defaultBBoxCenter},
                    { "bboxSize", defaultBBoxSize}
                });
            
            // Box
            s_parseInfoMap[Vrml97NodeName.Box] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "size", (SFVec3f, new V3f(2.0f, 2.0f, 2.0f)) }
                });

            // Collision
            s_parseInfoMap[Vrml97NodeName.Collision] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "children", fd(MFNode) },
                    { "collide", (SFBool, true) },
                    { "bboxCenter", defaultBBoxCenter},
                    { "bboxSize", defaultBBoxSize},
                    { "proxy", fd(SFNode) }
                });

            // Color
            s_parseInfoMap[Vrml97NodeName.Color] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "color", fd(MFColor) }
                });

            // ColorInterpolator
            s_parseInfoMap[Vrml97NodeName.ColorInterpolator] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "key", fd(MFFloat) },
                    { "keyValue", fd(MFColor) }
                });

            // Cone
            s_parseInfoMap[Vrml97NodeName.Cone] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "bottomRadius", (SFFloat, 1.0f) },
                    { "height", (SFFloat, 2.0f) },
                    { "side", (SFBool, true) },
                    { "bottom", (SFBool, true) }
                });

            // Coordinate
            s_parseInfoMap[Vrml97NodeName.Coordinate] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "point", fd(MFVec3f) }
                });

            // CoordinateInterpolator
            s_parseInfoMap[Vrml97NodeName.CoordinateInterpolator] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "key", fd(MFFloat) },
                    { "keyValue", fd(MFVec3f) }
                });

            // Cylinder
            s_parseInfoMap[Vrml97NodeName.Cylinder] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "bottom", (SFBool, true) },
                    { "height", (SFFloat, 2.0f) },
                    { "radius", (SFFloat, 1.0f) },
                    { "side", (SFBool, true) },
                    { "top", (SFBool, true) }
                });

            // CylinderSensor
            s_parseInfoMap[Vrml97NodeName.CylinderSensor] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "autoOffset", (SFBool, true) },
                    { "diskAngle", (SFFloat, 0.262f) },
                    { "enabled", (SFBool, true) },
                    { "maxAngle", (SFFloat, -1.0f) },
                    { "minAngle", (SFFloat, 0.0f) },
                    { "offset", (SFFloat, 0.0f) }
                });

            // DirectionalLight
            s_parseInfoMap[Vrml97NodeName.DirectionalLight] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "ambientIntensity", (SFFloat, 0.0f) },
                    { "color", (SFColor, C3f.White) },
                    { "direction", (SFVec3f, new V3f(0.0f, 0.0f, -1.0f)) },
                    { "intensity", (SFFloat, 1.0f) },
                    { "on", (SFBool, true) }
                });

            // ElevationGrid
            s_parseInfoMap[Vrml97NodeName.ElevationGrid] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "color", fd(SFNode) },
                    { "normal", fd(SFNode) },
                    { "texCoord", fd(SFNode) },
                    { "height", fd(MFFloat) },
                    { "ccw", (SFBool, true) },
                    { "colorPerVertex", (SFBool, true) },
                    { "creaseAngle", (SFFloat, 0.0f) },
                    { "normalPerVertex", (SFBool, true) },
                    { "solid", (SFBool, true) },
                    { "xDimension", (SFInt32, 0) },
                    { "xSpacing", (SFFloat, 1.0f) },
                    { "zDimension", (SFInt32, 0) },
                    { "zSpacing", (SFFloat, 1.0f) }
                });
     
            // Extrusion
            s_parseInfoMap[Vrml97NodeName.Extrusion] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "beginCap", (SFBool, true) },
                    { "ccw", (SFBool, true) },
                    { "convex", (SFBool, true) },
                    { "creaseAngle", (SFFloat, 0.0f) },
                    { "crossSection", (MFVec2f, new List<V2f>() {new V2f(1.0f, 1.0f), new V2f(1.0f, -1.0f), new V2f(-1.0f, -1.0f), new V2f(-1.0f, 1.0f), new V2f(1.0f, 1.0f) }) },
                    { "endCap", (SFBool, true) },
                    { "orientation", (MFRotation, new V4f(0.0f, 0.0f, 1.0f, 0.0f)) },
                    { "scale", (MFVec2f, new V2f(1.0f, 1.0f)) },
                    { "solid", (SFBool, true) },
                    { "spine", (MFVec3f, new List<V3f>() { V3f.Zero, new V3f(0.0f, 1.0f, 0.0f) }) }
                });

            // Fog
            s_parseInfoMap[Vrml97NodeName.Fog] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "color", (SFColor, C3f.White) },
                    { "fogType", (SFString, "LINEAR") },
                    { "visibilityRange", (SFFloat, 0.0f) }
                });

            // FontStyle
            s_parseInfoMap[Vrml97NodeName.FontStyle] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "family", (MFString, "SERIF") },
                    { "horizontal", (SFBool, true) },
                    { "justify", (MFString, "BEGIN") },
                    { "language", fd(SFString) },
                    { "leftToRight", (SFBool, true) },
                    { "size", (SFFloat, 1.0f) },
                    { "spacing", (SFFloat, 1.0f) },
                    { "style", (SFString, "PLAIN") },
                    { "topToBottom", (SFBool, true) }
                });

            // Group
            s_parseInfoMap[Vrml97NodeName.Group] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "children", fd(MFNode) },
                    { "bboxCenter", defaultBBoxCenter },
                    { "bboxSize", defaultBBoxSize }
                });

            // ImageTexture
            s_parseInfoMap[Vrml97NodeName.ImageTexture] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "url", fd(MFString) },
                    { "repeatS", (SFBool, true) },
                    { "repeatT", (SFBool, true) }
                });

            // IndexedFaceSet
            s_parseInfoMap[Vrml97NodeName.IndexedFaceSet] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "color", fd(SFNode) },
                    { "coord", fd(SFNode) },
                    { "normal", fd(SFNode) },
                    { "texCoord", fd(SFNode) },
                    { "ccw", (SFBool, true) },
                    { "colorIndex", fd(MFInt32) },
                    { "colorPerVertex", (SFBool, true) },
                    { "convex", (SFBool, true) },
                    { "coordIndex", fd(MFInt32) },
                    { "creaseAngle", (SFFloat, 0.0f) },
                    { "normalIndex", fd(MFInt32) },
                    { "normalPerVertex", (SFBool, true) },
                    { "solid", (SFBool, true) },
                    { "texCoordIndex", fd(MFInt32) },  
                    { "edgeSharpness", fd(MFFloat) },
                    { "edgeSharpnessIndex", fd(MFInt32) },
                    { "neighborMesh", fd(MFString) },
                    { "neighborIndex", fd(MFInt32) },
                    { "neighborSide", fd(MFInt32) },
                    { "neighborFace", fd(MFInt32) },
                    { "meshName", fd(SFString) },
                    { "topologyHoles", fd(SFInt32) }
                });

            // IndexedLineSet
            s_parseInfoMap[Vrml97NodeName.IndexedLineSet] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "color", fd(SFNode) },
                    { "coord", fd(SFNode) },
                    { "colorIndex", fd(MFInt32) },
                    { "colorPerVertex", (SFBool, true) },
                    { "coordIndex", fd(MFInt32) }
                });

            // Inline
            s_parseInfoMap[Vrml97NodeName.Inline] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "url", fd(MFString) },
                    { "bboxCenter", defaultBBoxCenter },
                    { "bboxSize", defaultBBoxSize }
                });

            // LOD
            s_parseInfoMap[Vrml97NodeName.LOD] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "level", fd(MFNode) },
                    { "center", defaultBBoxCenter },
                    { "range", fd(MFFloat) }
                });

            // Material
            s_parseInfoMap[Vrml97NodeName.Material] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "ambientIntensity", (SFFloat, 0.2f) },
                    { "diffuseColor", (SFColor, new C3f(0.8f, 0.8f, 0.8f)) },
                    { "emissiveColor", (SFColor, C3f.Black) },
                    { "shininess", (SFFloat, 0.2f) },
                    { "specularColor", (SFColor, C3f.Black) },
                    { "transparency", (SFFloat, 0.0f) }
                });

            // MovieTexture
            s_parseInfoMap[Vrml97NodeName.MovieTexture] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "loop", (SFBool, false) },
                    { "speed", (SFFloat, 1.0f) },
                    { "startTime", (SFTime, 1.0f) },
                    { "stopTime", (SFTime, 1.0f) },
                    { "url", fd(MFString) },
                    { "repeatS", (SFBool, true) },
                    { "repeatT", (SFBool, true) }
                });

            // NavigationInfo
            s_parseInfoMap[Vrml97NodeName.NavigationInfo] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "avatarSize", (MFFloat, new List<float>() {0.25f, 1.6f, 0.75f}) },
                    { "headlight", (SFBool, true) },
                    { "speed", (SFFloat, 1.0f) },
                    { "type", (MFString, new List<string>() {"WALK", "ANY"}) },
                    { "visibilityLimit", (SFFloat, 0.0f) }
                });

            // Normal
            s_parseInfoMap[Vrml97NodeName.Normal] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "vector", fd(MFVec3f) }
                });

            // NormalInterpolator
            s_parseInfoMap[Vrml97NodeName.NormalInterpolator] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "key", fd(MFFloat) },
                    { "keyValue", fd(MFVec3f) }
                });

            // OrientationInterpolator
            s_parseInfoMap[Vrml97NodeName.OrientationInterpolator] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "key", fd(MFFloat) },
                    { "keyValue", fd(MFRotation) }
                });

            // PixelTexture
            s_parseInfoMap[Vrml97NodeName.PixelTexture] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "image", (SFImage, new List<uint>() {0, 0, 0}) },
                    { "repeatS", (SFBool, true) },
                    { "repeatT", (SFBool, true) }
                });

            // PlaneSensor
            s_parseInfoMap[Vrml97NodeName.PlaneSensor] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "autoOffset", (SFBool, true) },
                    { "enabled", (SFBool, true) },
                    { "maxPosition", (SFVec2f, new V2f(-1.0f, -1.0f)) },
                    { "minPosition", (SFVec2f, V2f.Zero) },
                    { "offset", defaultBBoxCenter }
                });

            // PointLight
            s_parseInfoMap[Vrml97NodeName.PointLight] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "ambientIntensity", (SFFloat, 0.0f) },
                    { "attenuation", (SFVec3f, new V3f(1.0f, 0.0f, 0.0f)) },
                    { "color", (SFColor, C3f.White) },
                    { "intensity", (SFFloat, 1.0f) },
                    { "location", defaultBBoxCenter },
                    { "on", (SFBool, true) },
                    { "radius", (SFFloat, 100.0f) }
                });

            // PointSet
            s_parseInfoMap[Vrml97NodeName.PointSet] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "color", fd(SFNode) },
                    { "coord", fd(SFNode) }
                });

            // PositionInterpolator
            s_parseInfoMap[Vrml97NodeName.PositionInterpolator] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "key", fd(MFFloat) },
                    { "keyValue", fd(MFVec3f) }
                });

            // ProximitySensor
            s_parseInfoMap[Vrml97NodeName.ProximitySensor] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "center", defaultBBoxCenter },
                    { "size", defaultBBoxCenter },
                    { "enabled", (SFBool, true) }
                });

            // ScalarInterpolator
            s_parseInfoMap[Vrml97NodeName.ScalarInterpolator] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "key", fd(MFFloat) },
                    { "keyValue", fd(MFFloat) }
                });

            // Script
            // skipped

            // Shape
            s_parseInfoMap[Vrml97NodeName.Shape] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "appearance", fd(SFNode) },
                    { "geometry", fd(SFNode) },
                });

            // Sound
            s_parseInfoMap[Vrml97NodeName.Sound] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "direction", (SFVec3f, new V3f(0.0f, 0.0f, 1.0f)) },
                    { "intensity", (SFFloat, 1.0f) },
                    { "location", defaultBBoxCenter },
                    { "maxBack", (SFFloat, 10.0f) },
                    { "maxFront", (SFFloat, 10.0f) },
                    { "minBack", (SFFloat, 1.0f) },
                    { "minFront", (SFFloat, 1.0f) },
                    { "priority", (SFFloat, 0.0f) },
                    { "source", fd(SFNode) },
                    { "spatialize", (SFBool, true) }
                });

            // Sphere
            s_parseInfoMap[Vrml97NodeName.Sphere] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "radius", (SFFloat, 1.0f) }
                });

            // SphereSensor
            s_parseInfoMap[Vrml97NodeName.SphereSensor] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "autoOffset", (SFBool, true) },
                    { "enabled", (SFBool, true) },
                    { "offset", (SFRotation, new V4f(0.0f, 1.0f, 0.0f, 0.0f)) }
                });

            // SpotLight
            s_parseInfoMap[Vrml97NodeName.SpotLight] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "ambientIntensity", (SFFloat, 0.0f) },
                    { "attenuation", (SFVec3f, new V3f(1.0f, 0.0f, 0.0f)) },
                    { "beamWidth", (SFFloat, 1.570796f) },
                    { "color", (SFColor, C3f.White) },
                    { "cutOffAngle", (SFFloat, 0.785398f) },
                    { "direction", (SFVec3f, new V3f(0.0f, 0.0f, -1.0f)) },
                    { "intensity", (SFFloat, 1.0f) },
                    { "location", defaultBBoxCenter },
                    { "on", (SFBool, true) },
                    { "radius", (SFFloat, 100.0f) }
                });

            // Switch
            s_parseInfoMap[Vrml97NodeName.Switch] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "choice", fd(MFNode) },
                    { "whichChoice", (SFInt32, -1) }
                });

            // Text
            s_parseInfoMap[Vrml97NodeName.Text] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "string", fd(MFString) },
                    { "fontStyle", fd(SFNode) },
                    { "length", fd(MFFloat) },
                    { "maxExtent", (SFFloat, 0.0f) }
                });

            // TextureCoordinate
            s_parseInfoMap[Vrml97NodeName.TextureCoordinate] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "point", fd(MFVec2f) }
                });

            // TextureTransform
            s_parseInfoMap[Vrml97NodeName.TextureTransform] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "center", (SFVec2f, V2f.Zero) },
                    { "rotation", (SFFloat, 0.0f) },
                    { "scale", (SFVec2f, new V2f(1.0f, 1.0f)) },
                    { "translation", (SFVec2f, V2f.Zero) }
                });

            // TimeSensor
            s_parseInfoMap[Vrml97NodeName.TimeSensor] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "cycleInterval", (SFTime, 1.0f) },
                    { "enabled", (SFBool, true) },
                    { "loop", (SFBool, false) },
                    { "startTime", (SFTime, 0.0f) },
                    { "stopTime", (SFTime, 0.0f) }
                });

            // TouchSensor
            s_parseInfoMap[Vrml97NodeName.TouchSensor] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "enabled", (SFBool, true) }
                });

            // Transform
            s_parseInfoMap[Vrml97NodeName.Transform] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "center", defaultBBoxCenter },
                    { "children", fd(MFNode) },
                    { "rotation", (SFRotation, new V4f(0.0f, 0.0f, 1.0f, 0.0f)) },
                    { "scale", (SFVec3f, new V3f(1.0f, 1.0f, 1.0f)) },
                    { "scaleOrientation", (SFRotation, new V4f(0.0f, 0.0f, 1.0f, 0.0f)) },
                    { "translation", defaultBBoxCenter },
                    { "bboxCenter", defaultBBoxCenter },
                    { "bboxSize", defaultBBoxSize }
                });

            // Viewpoint
            s_parseInfoMap[Vrml97NodeName.Viewpoint] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "fieldOfView", (SFFloat, 0.785398f) },
                    { "jump", (SFBool, true) },
                    { "orientation", (SFRotation, new V4f(0.0f, 0.0f, 1.0f, 0.0f)) },
                    { "position", (SFVec3f, new V3f(0.0f, 0.0f, 10.0f)) },
                    { "description", fd(SFString) }
                });

            // VisibilitySensor
            s_parseInfoMap[Vrml97NodeName.VisibilitySensor] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "center", defaultBBoxCenter },
                    { "enabled", (SFBool, true) },
                    { "size", defaultBBoxCenter }
                });

            // WorldInfo
            s_parseInfoMap[Vrml97NodeName.WorldInfo] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "title", fd(SFString) },
                    { "info", fd(MFString) }
                });
        }

        private static SymMapBase ParseDEF(Tokenizer t)
        {
            var result = new SymMapBase();
            result["name"] = t.NextNameToken().ToString();
            result["node"] = ParseNode(t);
            return result;
        }

        private static SymMapBase ParseUSE(Tokenizer t)
        {
            var result = new SymMapBase();
            result["name"] = t.NextNameToken().ToString();
            return result;
        }

        private static SymMapBase ParseROUTE(Tokenizer t)
        {
            var result = new SymMapBase();

            // nodeNameId.eventOutId
            result["out"] = t.NextNameToken().ToString();
            // "TO"
            t.NextToken();
            // nodeNameId.eventInId
            result["in"] = t.NextNameToken().ToString();

            return result;
        }

        private static SymMapBase ParseNULL(Tokenizer t) => null;

        #endregion

        #region Helper functions.

        private static object ParseSFBool(Tokenizer t) => t.NextToken().ToBool();

        private static List<bool> ParseMFBool(Tokenizer t)
        {
            var result = new List<bool>();

            var token = t.NextToken();
            if (token.IsBracketOpen)
            {
                token = t.NextToken();
                while (!token.IsBracketClose)
                {
                    result.Add(token.ToBool());
                    token = t.NextToken();
                }
            }
            else
            {
                result.Add(token.ToBool());
            }

            return result;
        }

        private static object ParseSFFloat(Tokenizer t) => t.NextToken().ToFloat();

        private static List<float> ParseMFFloat(Tokenizer t)
        {
            var result = new List<float>();

            var token = t.NextToken();
            if (token.IsBracketOpen)
            {
                token = t.NextToken();
                while (!token.IsBracketClose)
                {
                    result.Add(token.ToFloat());
                    token = t.NextToken();
                }
            }
            else
            {
                result.Add(token.ToFloat());
            }

            return result;
        }

        private static List<uint> ParseSFImage(Tokenizer t)
        {
            var result = new List<uint>
            {
                t.NextToken().ToUInt32(),   // width
                t.NextToken().ToUInt32(),   // height
                t.NextToken().ToUInt32()   // num components
            };

            uint imax = result[0] * result[1];
            for (uint i = 0; i < imax; i++)
            {
                result.Add(t.NextToken().ToUInt32());
            }

            return result;
        }

        private static object ParseSFInt32(Tokenizer t) => t.NextToken().ToInt32();

        private static List<int> ParseMFInt32(Tokenizer t)
        {
            var result = new List<int>();

            var token = t.NextToken();
            if (token.IsBracketOpen)
            {
                token = t.NextToken();
                while (!token.IsBracketClose)
                {
                    result.Add(token.ToInt32());
                    token = t.NextToken();
                }
            }
            else
            {
                result.Add(token.ToInt32());
            }

            return result;
        }

        private static SymMapBase ParseSFNode(Tokenizer t) => ParseNode(t);

        private static List<SymMapBase> ParseMFNode(Tokenizer t)
        {
            var result = new List<SymMapBase>();

            var token = t.NextToken();
            if (token.IsBracketOpen)
            {
                token = t.NextToken();
                while (!token.IsBracketClose)
                {
                    t.PushBack(token);
                    result.Add(ParseNode(t));
                    token = t.NextToken();
                }
            }
            else
            {
                t.PushBack(token);
                result.Add(ParseNode(t));
            }

            return result;
        }

        private static object ParseSFRotation(Tokenizer t)
        {
            var x = t.NextToken().ToFloat();
            var y = t.NextToken().ToFloat();
            var z = t.NextToken().ToFloat();
            var w = t.NextToken().ToFloat();
            return new V4f(x, y, z, w);
        }

        private static List<V4f> ParseMFRotation(Tokenizer t)
        {
            var result = new List<V4f>();

            var token = t.NextToken();
            if (token.IsBracketOpen)
            {
                token = t.NextToken();
                while (!token.IsBracketClose)
                {
                    var x = token.ToFloat();
                    var y = t.NextToken().ToFloat();
                    var z = t.NextToken().ToFloat();
                    var w = t.NextToken().ToFloat();
                    result.Add(new V4f(x, y, z, w));

                    token = t.NextToken();
                }
            }
            else
            {
                var x = token.ToFloat();
                var y = t.NextToken().ToFloat();
                var z = t.NextToken().ToFloat();
                var w = t.NextToken().ToFloat();
                result.Add(new V4f(x, y, z, w));
            }

            return result;
        }

        private static string ParseSFString(Tokenizer t)
            => t.NextToken().GetCheckedUnquotedString();

        private static List<string> ParseMFString(Tokenizer t)
        {
            var result = new List<string>();

            var token = t.NextToken();
            if (token.IsBracketOpen)
            {
                token = t.NextToken();
                while (!token.IsBracketClose)
                {
                    result.Add(token.GetCheckedUnquotedString());
                    token = t.NextToken();
                }
            }
            else
            {
                result.Add(token.GetCheckedUnquotedString());
            }

            return result;
        }

        private static object ParseSFVec2f(Tokenizer t)
        {
            var x = t.NextToken().ToFloat();
            var y = t.NextToken().ToFloat();
            return new V2f(x, y);
        }

        private static List<V2f> ParseMFVec2f(Tokenizer t)
        {
            var result = new List<V2f>();

            var token = t.NextToken();
            if (token.IsBracketOpen)
            {
                token = t.NextToken();
                while (!token.IsBracketClose)
                {
                    float x = token.ToFloat();
                    float y = t.NextToken().ToFloat();
                    result.Add(new V2f(x, y));

                    token = t.NextToken();
                }
            }
            else
            {
                float x = token.ToFloat();
                float y = t.NextToken().ToFloat();
                result.Add(new V2f(x, y));
            }

            return result;
        }

        private static object ParseSFVec3f(Tokenizer t)
        {
            var x = t.NextToken().ToFloat();
            var y = t.NextToken().ToFloat();
            var z = t.NextToken().ToFloat();
            return new V3f(x, y, z);
        }

        private static List<V3f> ParseMFVec3f(Tokenizer t)
        {
            var result = new List<V3f>();

            var token = t.NextToken();
            if (token.IsBracketOpen)
            {
                token = t.NextToken();
                while (!token.IsBracketClose)
                {
                    var x = token.ToFloat();
                    var y = t.NextToken().ToFloat();
                    var z = t.NextToken().ToFloat();
                    result.Add(new V3f(x, y, z));

                    token = t.NextToken();
                }
            }
            else
            {
                var x = token.ToFloat();
                var y = t.NextToken().ToFloat();
                var z = t.NextToken().ToFloat();
                result.Add(new V3f(x, y, z));
            }

            return result;
        }

        private static object ParseSFColor(Tokenizer t)
        {
            var r = t.NextToken().ToFloat();
            var g = t.NextToken().ToFloat();
            var b = t.NextToken().ToFloat();
            return new C3f(r, g, b);
        }

        private static List<C3f> ParseMFColor(Tokenizer t)
        {
            var result = new List<C3f>();

            var token = t.NextToken();
            if (token.IsBracketOpen)
            {
                token = t.NextToken();
                while (!token.IsBracketClose)
                {
                    var r = token.ToFloat();
                    var g = t.NextToken().ToFloat();
                    var b = t.NextToken().ToFloat();
                    result.Add(new C3f(r, g, b));

                    token = t.NextToken();
                }
            }
            else
            {
                var r = token.ToFloat();
                var g = t.NextToken().ToFloat();
                var b = t.NextToken().ToFloat();
                result.Add(new C3f(r, g, b));
            }

            return result;
        }

        



        private static void ExpectBraceOpen(Tokenizer t)
        {
            var token = t.NextToken();
            if (token.IsBraceOpen) return;

            throw new ParseException(
                "Token '{' expected. Found " + token.ToString() + " instead!"
                );
        }

        private static void ExpectBraceClose(Tokenizer t)
        {
            var token = t.NextToken();
            if (token.IsBraceClose) return;

            throw new ParseException(
                "Token '}' expected. Found " + token.ToString() + " instead!"
                );
        }

        #endregion

        #region Internal stuff.

        private static SymMapBase ParseNode(Tokenizer t)
        {
            // Next token is expected to be a Vrml97 node type.
            var nodeType = t.NextToken().ToString();
            if (nodeType == null) return null;

            SymMapBase node;

            // If a field description is available for this type,
            // then use the generic node parser, else use the custom
            // parse function.
            if (s_parseInfoMap.ContainsKey(nodeType))
            {
                var info = s_parseInfoMap[nodeType];
                node = (info.FieldDefs == null) ?
                    info.NodeParser(t) :
                    ParseGenericNode(t, info);
            }
            else
            {
                // unknown node type
                node = ParseUnknownNode(t);
            }

            if (node != null)
                node.TypeName = nodeType;

            return node;
        }


        /**
         * Specifies how to parse a node.
         **/
        private struct NodeParseInfo
        {
            private NodeParser m_parseFunction;
            public readonly SymbolDict<(FieldParser, object)> FieldDefs;

            public NodeParseInfo(NodeParser parseFunction)
                : this(parseFunction, null)
            { }

            public NodeParseInfo(
                SymbolDict<(FieldParser, object)> fields)
                : this(null, fields)
            { }

            public NodeParseInfo(
                NodeParser parseFunction,
                SymbolDict<(FieldParser, object)> fields)
            {
                m_parseFunction = parseFunction;
                FieldDefs = fields;
            }

            public NodeParser NodeParser { get { return m_parseFunction; } }
            public FieldParser FieldParser(string fieldName)
            {
                if (fieldName == "ROUTE") return new FieldParser(ParseROUTE);
                return FieldDefs[fieldName].Item1;
            }
            public object DefaultValue(string fieldName)
            {
                return FieldDefs[fieldName].Item2;
            }
        }

        private static SymMapBase ParseGenericNode(
            Tokenizer t,
            NodeParseInfo info
            )
        {
            var result = new SymMapBase();
            ExpectBraceOpen(t);

            // populate fields with default values
            foreach (var kvp in info.FieldDefs)
            {
                if (kvp.Value.Item2 == null) continue;
                result[kvp.Key] = kvp.Value.Item2;
            }

            Tokenizer.Token token = t.NextToken();
            while (!token.IsBraceClose)
            {
                string fieldName = token.ToString();
                result[fieldName] = info.FieldParser(fieldName)(t);

                token = t.NextToken();
                Thread.Sleep(0);
            }

            return result;
        }

        private static SymMapBase ParseUnknownNode(Tokenizer t)
        {
            ExpectBraceOpen(t);
            var level = 1;

            var sb = new StringBuilder("{");

            do
            {
                var token = t.NextToken();
                sb.Append(" " + token);

                if (token.IsBraceOpen) level++;
                if (token.IsBraceClose) level--;
            }
            while (level > 0);

            var result = new SymMapBase();
            result["unknownNode"] = true;
            result["content"] = sb.ToString();
            return result;
        }

        /// <summary>
        /// Disposed the input stream: Either the FileStream created by
        /// FromFile or the Stream passed when creating.
        /// </summary>
        public void Dispose()
        {
            m_inputStream.Dispose();
        }

        private delegate SymMapBase NodeParser(Tokenizer t);
        public delegate object FieldParser(Tokenizer t);

        private static SymbolDict<NodeParseInfo> s_parseInfoMap;

        private SymMapBase m_result = new SymMapBase();
        private Tokenizer m_tokenizer;

        #endregion
    }
}
