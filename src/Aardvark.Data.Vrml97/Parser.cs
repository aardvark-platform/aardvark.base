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
    /// Vrml97 parser.
    /// Creates a parse tree from a file, or a stream reader.
    /// 
    /// Example:
    /// Parser parser = new Parser("myVrmlFile.wrl");
    /// SymMapBase parseTree = parser.Perform();
    /// 
    /// </summary>
    internal class Parser
    {
        #region Public interface.

        public static Vrml97Scene FromFile(string fileName)
            => new Parser(fileName).Perform();

        public static Vrml97Scene FromStream(Stream stream, string fileName)
            => new Parser(stream, fileName).Perform();

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
            m_tokenizer = new Tokenizer(input);
        }

        /// <summary>
        /// Constructs a Parser for the given file.
        /// In order to actually parse the data, call the
        /// Perform method, which returns a SymMapBase
       ///  containing the parse tree.
        /// </summary>
        /// <param name="fileName">Input filename.</param>
        public Parser(string fileName)
        {
            m_result.TypeName = Vrml97Sym.Vrml97;
            m_result[Vrml97Sym.filename] = fileName;

            var fs = new FileStream(
                fileName,
                FileMode.Open, FileAccess.Read, FileShare.Read,
                4096, false
                );

            m_tokenizer = new Tokenizer(fs);
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

        /** Static constructor. */
        static Parser()
        {
            var SFBool = new FieldParser(ParseSFBool);
            //var MFBool = new FieldParser(ParseMFBool);
            var SFColor = new FieldParser(ParseSFColor);
            var MFColor = new FieldParser(ParseMFColor);
            var SFFloat = new FieldParser(ParseSFFloat);
            var MFFloat = new FieldParser(ParseMFFloat);
            var SFImage = new FieldParser(ParseSFImage);
            var SFInt32 = new FieldParser(ParseSFInt32);
            var MFInt32 = new FieldParser(ParseMFInt32);
            var SFNode = new FieldParser(ParseSFNode);
            var MFNode = new FieldParser(ParseMFNode);
            var SFRotation = new FieldParser(ParseSFRotation);
            var MFRotation = new FieldParser(ParseMFRotation);
            var SFString = new FieldParser(ParseSFString);
            var MFString = new FieldParser(ParseMFString);
            var SFTime = new FieldParser(ParseSFFloat);
            //var MFTime = new FieldParser(ParseMFFloat);
            var SFVec2f = new FieldParser(ParseSFVec2f);
            var MFVec2f = new FieldParser(ParseMFVec2f);
            var SFVec3f = new FieldParser(ParseSFVec3f);
            var MFVec3f = new FieldParser(ParseMFVec3f);

            //            Dictionary<string, (FieldParser, object)> fields;

            // Lookup table for Vrml97 node types.
            // For each node type a NodeParseInfo entry specifies how
            // to handle this kind of node.
            m_parseInfoMap = new SymbolDict<NodeParseInfo>
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

            (FieldParser, object) fdd(FieldParser fp, object obj) => (fp, obj);
            (FieldParser, object) fd(FieldParser fp) => (fp, null);

            // Anchor
            m_parseInfoMap["Anchor"] = new NodeParseInfo(
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
            m_parseInfoMap["Appearance"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "material", fd(SFNode) },
                    { "texture", fd(SFNode) },
                    { "textureTransform", fd(SFNode) }
                });

            // AudioClip
            m_parseInfoMap["AudioClip"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "description", fd(SFString) },
                    { "loop", fdd(SFBool, false) },
                    { "pitch", fdd(SFFloat, 1.0f) },
                    { "startTime", fdd(SFTime, 0.0f)},
                    { "stopTime", fdd(SFTime, 0.0f)},
                    { "url", fd(MFString)}
                });

            // Background
            m_parseInfoMap["Background"] = new NodeParseInfo(
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
                    { "skyColor", fdd(MFColor, C3f.Black) }
                });

            // Billboard
            m_parseInfoMap["Billboard"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "axisOfRotation", fdd(SFVec3f, new V3f(0.0f, 1.0f, 0.0f)) },
                    { "children", fd(MFNode) },
                    { "bboxCenter", defaultBBoxCenter},
                    { "bboxSize", defaultBBoxSize}
                });
            
            // Box
            m_parseInfoMap["Box"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "size", fdd(SFVec3f, new V3f(2.0f, 2.0f, 2.0f)) }
                });

            // Collision
            m_parseInfoMap["Collision"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "children", fd(MFNode) },
                    { "collide", fdd(SFBool, true) },
                    { "bboxCenter", defaultBBoxCenter},
                    { "bboxSize", defaultBBoxSize},
                    { "proxy", fd(SFNode) }
                });

            // Color
            m_parseInfoMap["Color"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "color", fd(MFColor) }
                });

            // ColorInterpolator
            m_parseInfoMap["ColorInterpolator"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "key", fd(MFFloat) },
                    { "keyValue", fd(MFColor) }
                });

            // Cone
            m_parseInfoMap["Cone"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "bottomRadius", fdd(SFFloat, 1.0f) },
                    { "height", fdd(SFFloat, 2.0f) },
                    { "side", fdd(SFBool, true) },
                    { "bottom", fdd(SFBool, true) }
                });

            // Coordinate
            m_parseInfoMap["Coordinate"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "point", fd(MFVec3f) }
                });

            // CoordinateInterpolator
            m_parseInfoMap["CoordinateInterpolator"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "key", fd(MFFloat) },
                    { "keyValue", fd(MFVec3f) }
                });

            // Cylinder
            m_parseInfoMap["Cylinder"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "bottom", fdd(SFBool, true) },
                    { "height", fdd(SFFloat, 2.0f) },
                    { "radius", fdd(SFFloat, 1.0f) },
                    { "side", fdd(SFBool, true) },
                    { "top", fdd(SFBool, true) }
                });

            // CylinderSensor
            m_parseInfoMap["CylinderSensor"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "autoOffset", fdd(SFBool, true) },
                    { "diskAngle", fdd(SFFloat, 0.262f) },
                    { "enabled", fdd(SFBool, true) },
                    { "maxAngle", fdd(SFFloat, -1.0f) },
                    { "minAngle", fdd(SFFloat, 0.0f) },
                    { "offset", fdd(SFFloat, 0.0f) }
                });

            // DirectionalLight
            m_parseInfoMap["DirectionalLight"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "ambientIntensity", fdd(SFFloat, 0.0f) },
                    { "color", fdd(SFColor, C3f.White) },
                    { "direction", fdd(SFVec3f, new V3f(0.0f, 0.0f, -1.0f)) },
                    { "intensity", fdd(SFFloat, 1.0f) },
                    { "on", fdd(SFBool, true) }
                });

            // ElevationGrid
            m_parseInfoMap["ElevationGrid"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "color", fd(SFNode) },
                    { "normal", fd(SFNode) },
                    { "texCoord", fd(SFNode) },
                    { "height", fd(MFFloat) },
                    { "ccw", fdd(SFBool, true) },
                    { "colorPerVertex", fdd(SFBool, true) },
                    { "creaseAngle", fdd(SFFloat, 0.0f) },
                    { "normalPerVertex", fdd(SFBool, true) },
                    { "solid", fdd(SFBool, true) },
                    { "xDimension", fdd(SFInt32, 0) },
                    { "xSpacing", fdd(SFFloat, 1.0f) },
                    { "zDimension", fdd(SFInt32, 0) },
                    { "zSpacing", fdd(SFFloat, 1.0f) }
                });
     
            // Extrusion
            m_parseInfoMap["Extrusion"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "beginCap", fdd(SFBool, true) },
                    { "ccw", fdd(SFBool, true) },
                    { "convex", fdd(SFBool, true) },
                    { "creaseAngle", fdd(SFFloat, 0.0f) },
                    { "crossSection", fdd(MFVec2f, new List<V2f>() {new V2f(1.0f, 1.0f), new V2f(1.0f, -1.0f), new V2f(-1.0f, -1.0f), new V2f(-1.0f, 1.0f), new V2f(1.0f, 1.0f) }) },
                    { "endCap", fdd(SFBool, true) },
                    { "orientation", fdd(MFRotation, new V4f(0.0f, 0.0f, 1.0f, 0.0f)) },
                    { "scale", fdd(MFVec2f, new V2f(1.0f, 1.0f)) },
                    { "solid", fdd(SFBool, true) },
                    { "spine", fdd(MFVec3f, new List<V3f>() { V3f.Zero, new V3f(0.0f, 1.0f, 0.0f) }) }
                });

            // Fog
            m_parseInfoMap["Fog"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "color", fdd(SFColor, C3f.White) },
                    { "fogType", fdd(SFString, "LINEAR") },
                    { "visibilityRange", fdd(SFFloat, 0.0f) }
                });

            // FontStyle
            m_parseInfoMap["FontStyle"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "family", fdd(MFString, "SERIF") },
                    { "horizontal", fdd(SFBool, true) },
                    { "justify", fdd(MFString, "BEGIN") },
                    { "language", fd(SFString) },
                    { "leftToRight", fdd(SFBool, true) },
                    { "size", fdd(SFFloat, 1.0f) },
                    { "spacing", fdd(SFFloat, 1.0f) },
                    { "style", fdd(SFString, "PLAIN") },
                    { "topToBottom", fdd(SFBool, true) }
                });

            // Group
            m_parseInfoMap["Group"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "children", fd(MFNode) },
                    { "bboxCenter", defaultBBoxCenter },
                    { "bboxSize", defaultBBoxSize }
                });

            // ImageTexture
            m_parseInfoMap["ImageTexture"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "url", fd(MFString) },
                    { "repeatS", fdd(SFBool, true) },
                    { "repeatT", fdd(SFBool, true) }
                });

            // IndexedFaceSet
            m_parseInfoMap["IndexedFaceSet"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "color", fd(SFNode) },
                    { "coord", fd(SFNode) },
                    { "normal", fd(SFNode) },
                    { "texCoord", fd(SFNode) },
                    { "ccw", fdd(SFBool, true) },
                    { "colorIndex", fd(MFInt32) },
                    { "colorPerVertex", fdd(SFBool, true) },
                    { "convex", fdd(SFBool, true) },
                    { "coordIndex", fd(MFInt32) },
                    { "creaseAngle", fdd(SFFloat, 0.0f) },
                    { "normalIndex", fd(MFInt32) },
                    { "normalPerVertex", fdd(SFBool, true) },
                    { "solid", fdd(SFBool, true) },
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
            m_parseInfoMap["IndexedLineSet"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "color", fd(SFNode) },
                    { "coord", fd(SFNode) },
                    { "colorIndex", fd(MFInt32) },
                    { "colorPerVertex", fdd(SFBool, true) },
                    { "coordIndex", fd(MFInt32) }
                });

            // Inline
            m_parseInfoMap["Inline"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "url", fd(MFString) },
                    { "bboxCenter", defaultBBoxCenter },
                    { "bboxSize", defaultBBoxSize }
                });

            // LOD
            m_parseInfoMap["LOD"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "level", fd(MFNode) },
                    { "center", defaultBBoxCenter },
                    { "range", fd(MFFloat) }
                });

            // Material
            m_parseInfoMap["Material"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "ambientIntensity", fdd(SFFloat, 0.2f) },
                    { "diffuseColor", fdd(SFColor, new C3f(0.8f, 0.8f, 0.8f)) },
                    { "emissiveColor", fdd(SFColor, C3f.Black) },
                    { "shininess", fdd(SFFloat, 0.2f) },
                    { "specularColor", fdd(SFColor, C3f.Black) },
                    { "transparency", fdd(SFFloat, 0.0f) }
                });

            // MovieTexture
            m_parseInfoMap["MovieTexture"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "loop", fdd(SFBool, false) },
                    { "speed", fdd(SFFloat, 1.0f) },
                    { "startTime", fdd(SFTime, 1.0f) },
                    { "stopTime", fdd(SFTime, 1.0f) },
                    { "url", fd(MFString) },
                    { "repeatS", fdd(SFBool, true) },
                    { "repeatT", fdd(SFBool, true) }
                });

            // NavigationInfo
            m_parseInfoMap["NavigationInfo"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "avatarSize", fdd(MFFloat, new List<float>() {0.25f, 1.6f, 0.75f}) },
                    { "headlight", fdd(SFBool, true) },
                    { "speed", fdd(SFFloat, 1.0f) },
                    { "type", fdd(MFString, new List<string>() {"WALK", "ANY"}) },
                    { "visibilityLimit", fdd(SFFloat, 0.0f) }
                });

            // Normal
            m_parseInfoMap["Normal"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "vector", fd(MFVec3f) }
                });

            // NormalInterpolator
            m_parseInfoMap["NormalInterpolator"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "key", fd(MFFloat) },
                    { "keyValue", fd(MFVec3f) }
                });

            // OrientationInterpolator
            m_parseInfoMap["OrientationInterpolator"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "key", fd(MFFloat) },
                    { "keyValue", fd(MFRotation) }
                });

            // PixelTexture
            m_parseInfoMap["PixelTexture"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "image", fdd(SFImage, new List<uint>() {0, 0, 0}) },
                    { "repeatS", fdd(SFBool, true) },
                    { "repeatT", fdd(SFBool, true) }
                });

            // PlaneSensor
            m_parseInfoMap["PlaneSensor"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "autoOffset", fdd(SFBool, true) },
                    { "enabled", fdd(SFBool, true) },
                    { "maxPosition", fdd(SFVec2f, new V2f(-1.0f, -1.0f)) },
                    { "minPosition", fdd(SFVec2f, V2f.Zero) },
                    { "offset", defaultBBoxCenter }
                });

            // PointLight
            m_parseInfoMap["PointLight"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "ambientIntensity", fdd(SFFloat, 0.0f) },
                    { "attenuation", fdd(SFVec3f, new V3f(1.0f, 0.0f, 0.0f)) },
                    { "color", fdd(SFColor, C3f.White) },
                    { "intensity", fdd(SFFloat, 1.0f) },
                    { "location", defaultBBoxCenter },
                    { "on", fdd(SFBool, true) },
                    { "radius", fdd(SFFloat, 100.0f) }
                });

            // PointSet
            m_parseInfoMap["PointSet"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "color", fd(SFNode) },
                    { "coord", fd(SFNode) }
                });

            // PositionInterpolator
            m_parseInfoMap["PositionInterpolator"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "key", fd(MFFloat) },
                    { "keyValue", fd(MFVec3f) }
                });

            // ProximitySensor
            m_parseInfoMap["ProximitySensor"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "center", defaultBBoxCenter },
                    { "size", defaultBBoxCenter },
                    { "enabled", fdd(SFBool, true) }
                });

            // ScalarInterpolator
            m_parseInfoMap["ScalarInterpolator"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "key", fd(MFFloat) },
                    { "keyValue", fd(MFFloat) }
                });

            // Script
            // skipped

            // Shape
            m_parseInfoMap["Shape"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "appearance", fd(SFNode) },
                    { "geometry", fd(SFNode) },
                });

            // Sound
            m_parseInfoMap["Sound"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "direction", fdd(SFVec3f, new V3f(0.0f, 0.0f, 1.0f)) },
                    { "intensity", fdd(SFFloat, 1.0f) },
                    { "location", defaultBBoxCenter },
                    { "maxBack", fdd(SFFloat, 10.0f) },
                    { "maxFront", fdd(SFFloat, 10.0f) },
                    { "minBack", fdd(SFFloat, 1.0f) },
                    { "minFront", fdd(SFFloat, 1.0f) },
                    { "priority", fdd(SFFloat, 0.0f) },
                    { "source", fd(SFNode) },
                    { "spatialize", fdd(SFBool, true) }
                });

            // Sphere
            m_parseInfoMap["Sphere"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "radius", fdd(SFFloat, 1.0f) }
                });

            // SphereSensor
            m_parseInfoMap["SphereSensor"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "autoOffset", fdd(SFBool, true) },
                    { "enabled", fdd(SFBool, true) },
                    { "offset", fdd(SFRotation, new V4f(0.0f, 1.0f, 0.0f, 0.0f)) }
                });

            // SpotLight
            m_parseInfoMap["SpotLight"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "ambientIntensity", fdd(SFFloat, 0.0f) },
                    { "attenuation", fdd(SFVec3f, new V3f(1.0f, 0.0f, 0.0f)) },
                    { "beamWidth", fdd(SFFloat, 1.570796f) },
                    { "color", fdd(SFColor, C3f.White) },
                    { "cutOffAngle", fdd(SFFloat, 0.785398f) },
                    { "direction", fdd(SFVec3f, new V3f(0.0f, 0.0f, -1.0f)) },
                    { "intensity", fdd(SFFloat, 1.0f) },
                    { "location", defaultBBoxCenter },
                    { "on", fdd(SFBool, true) },
                    { "radius", fdd(SFFloat, 100.0f) }
                });

            // Switch
            m_parseInfoMap["Switch"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "choice", fd(MFNode) },
                    { "whichChoice", fdd(SFInt32, -1) }
                });

            // Text
            m_parseInfoMap["Text"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "string", fd(MFString) },
                    { "fontStyle", fd(SFNode) },
                    { "length", fd(MFFloat) },
                    { "maxExtent", fdd(SFFloat, 0.0f) }
                });

            // TextureCoordinate
            m_parseInfoMap["TextureCoordinate"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "point", fd(MFVec2f) }
                });

            // TextureTransform
            m_parseInfoMap["TextureTransform"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "center", fdd(SFVec2f, V2f.Zero) },
                    { "rotation", fdd(SFFloat, 0.0f) },
                    { "scale", fdd(SFVec2f, new V2f(1.0f, 1.0f)) },
                    { "translation", fdd(SFVec2f, V2f.Zero) }
                });

            // TimeSensor
            m_parseInfoMap["TimeSensor"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "cycleInterval", fdd(SFTime, 1.0f) },
                    { "enabled", fdd(SFBool, true) },
                    { "loop", fdd(SFBool, false) },
                    { "startTime", fdd(SFTime, 0.0f) },
                    { "stopTime", fdd(SFTime, 0.0f) }
                });

            // TouchSensor
            m_parseInfoMap["TouchSensor"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "enabled", fdd(SFBool, true) }
                });

            // Transform
            m_parseInfoMap["Transform"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "center", defaultBBoxCenter },
                    { "children", fd(MFNode) },
                    { "rotation", fdd(SFRotation, new V4f(0.0f, 0.0f, 1.0f, 0.0f)) },
                    { "scale", fdd(SFVec3f, new V3f(1.0f, 1.0f, 1.0f)) },
                    { "scaleOrientation", fdd(SFRotation, new V4f(0.0f, 0.0f, 1.0f, 0.0f)) },
                    { "translation", defaultBBoxCenter },
                    { "bboxCenter", defaultBBoxCenter },
                    { "bboxSize", defaultBBoxSize }
                });

            // Viewpoint
            m_parseInfoMap["Viewpoint"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "fieldOfView", fdd(SFFloat, 0.785398f) },
                    { "jump", fdd(SFBool, true) },
                    { "orientation", fdd(SFRotation, new V4f(0.0f, 0.0f, 1.0f, 0.0f)) },
                    { "position", fdd(SFVec3f, new V3f(0.0f, 0.0f, 10.0f)) },
                    { "description", fd(SFString) }
                });

            // VisibilitySensor
            m_parseInfoMap["VisibilitySensor"] = new NodeParseInfo(
                new SymbolDict<(FieldParser, object)>()
                {
                    { "center", defaultBBoxCenter },
                    { "enabled", fdd(SFBool, true) },
                    { "size", defaultBBoxCenter }
                });

            // WorldInfo
            m_parseInfoMap["WorldInfo"] = new NodeParseInfo(
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
            if (m_parseInfoMap.ContainsKey(nodeType))
            {
                var info = m_parseInfoMap[nodeType];
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

        private delegate SymMapBase NodeParser(Tokenizer t);
        private delegate object FieldParser(Tokenizer t);

        private static SymbolDict<NodeParseInfo> m_parseInfoMap;

        private SymMapBase m_result = new SymMapBase();
        private Tokenizer m_tokenizer;

        #endregion
    }
}
