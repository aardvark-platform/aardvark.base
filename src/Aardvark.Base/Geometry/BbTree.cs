using Aardvark.Base;
using System;
using Aardvark.Base.Sorting;

namespace Aardvark.Base
{
    public struct BbTreeHit
    {
        public int NodeIndex;
        public double RayT;
        
        public BbTreeHit(int nodeIndex, double rayT)
        {
            NodeIndex = nodeIndex;
            RayT = rayT;
        }
    }
    
    
    public class BbTree
    {
        private int m_nodeCount;
        private Box3d m_box;
        private int[] m_indexArray;
        private int[] m_leafArray;
        private Box3d[] m_boxes;

        private Box3dAndFlags[] m_combinedBoxArray;
        public Box3d[] m_leftBoxArray;
        public Box3d[] m_rightBoxArray;

        [Flags]
        public enum BuildFlags
        {
            None,
            CreateBoxArrays = 0x01,
            CreateCombinedArray = 0x02,

            LeafLimit01         = 0x010,
            LeafLimit04         = 0x040,
            LeafLimit08         = 0x080,
            LeafLimit16         = 0x100,
            LeafLimitMask       = 0xff0,

            Default = CreateBoxArrays | LeafLimit01
        }

        #region Constructor

        public BbTree(Box3d[] boundingBoxes, BuildFlags buildFlags = BuildFlags.Default,
                      int[] countArray = null)
        {
            var count = boundingBoxes.Length;
            m_combinedBoxArray = null;
            m_leftBoxArray = null;
            m_rightBoxArray = null;

            Report.BeginTimed(c_reportVerbosity, "building bb-tree [{0}]", count);
            if (count > 1)
            {
                m_boxes = boundingBoxes;
                m_nodeCount = count - 1;
                var createParams = new InParams
                {
                    LeafLimit = Fun.Max(1, (int)(buildFlags & BuildFlags.LeafLimitMask) >> 4),
                    LeafCount = 0,
                    BuildFlags = buildFlags,
                    NodeCount = 0,
                    BbArray = boundingBoxes,
                    CountArray = countArray,
                };
                if (createParams.LeafLimit > 1)
                    m_leafArray = new int[2 * count];

                m_indexArray = new int[2 * m_nodeCount];
                if ((buildFlags & BuildFlags.CreateCombinedArray) != 0)
                    m_combinedBoxArray = new Box3dAndFlags[m_nodeCount];
                if ((buildFlags & BuildFlags.CreateBoxArrays) != 0)
                {
                    m_leftBoxArray = new Box3d[m_nodeCount];
                    m_rightBoxArray = new Box3d[m_nodeCount];
                }
                m_box = new Box3d(createParams.BbArray);

                Create(createParams, new int[count].SetByIndex(i => i), 0, count, m_box);
                if (createParams.LeafLimit > 1)
                    m_leafArray = m_leafArray.Copy(createParams.LeafCount);
            }
            else
            {
                m_nodeCount = 0;
                m_box = count > 0 ? new Box3d(boundingBoxes) : Box3d.Invalid;
            }
            Report.End(c_reportVerbosity);
        }

        #endregion

        #region Properties

        public int[] IndexArray { get { return m_indexArray; } }
        public int[] LeafArray { get { return m_leafArray; } }
        public int NodeCount { get { return m_nodeCount; } }
        public Box3d Box3d { get { return m_box; } }
        public Box3d[] Boxes { get { return m_boxes; } }
        public Box3d[] LeftBoxArray { get { return m_leftBoxArray; } }
        public Box3d[] RightBoxArray { get { return m_rightBoxArray; } }

        public int GetLeft(int i) { return m_indexArray[i * 2]; }
        public int GetRight(int i) { return m_indexArray[i * 2 + 1]; }

        #endregion

        #region Constants

        private const double c_splitPenalty = 1.0; // 2.0 still OK, but gets slow beyond that
        private const int c_reportVerbosity = 2;

        #endregion

        #region Construction

        private class InParams
        {
            public BuildFlags BuildFlags;
            public int NodeCount;
            public int LeafLimit;
            public Box3d[] BbArray;
            public int[] CountArray;
            public int LeafCount;
        }

        private int CalculateSplit(
                InParams inParams,
                int[] indexArray,
                int start,
                int count,
                Box3d box,
                out int[] sortedIndexArray,
                out Box3d leftBox,
                out Box3d rightBox)
        {
            int bestLeft = 0;
            var bestCost = double.MaxValue;
            var bestLeftBox = Box3d.Invalid;
            var bestRightBox = Box3d.Invalid;
            var bestDim = -1;
            double invBoxArea = box.SurfaceArea > Constant<double>.PositiveTinyValue
                                    ? 1.0 / box.SurfaceArea : 0.0;

            var iaa = new int[3][].SetByIndex(i => indexArray.Copy(start, count));

            for (int d = 0; d < 3; d++)
            {
                var ia = iaa[d];
                ia.PermutationQuickSortAscending(inParams.BbArray, (ba, i) => ba[i].Center[d]);

                var bbLeftArray = ia.ScanLeft(Box3d.Invalid, (b, i) => new Box3d(b, inParams.BbArray[i]));
                var bbRightArray = ia.ScanRight((i, b) => new Box3d(inParams.BbArray[i], b), Box3d.Invalid);

                long[] cLeftArray = null, cRightArray = null;
                if (inParams.CountArray != null)
                {
                    cLeftArray = ia.ScanLeft(0L, (c, i) => c + inParams.CountArray[i]);
                    cRightArray = ia.ScanRight((i, c) => inParams.CountArray[i] + c, 0L);
                }
                for (int s = 1; s < count; s++)
                {
                    var lBox = bbLeftArray[s-1]; var leftP = lBox.SurfaceArea * invBoxArea;
                    var rBox = bbRightArray[s]; var rightP = rBox.SurfaceArea * invBoxArea;
                    long lCount = cLeftArray != null ? cLeftArray[s - 1] : s;
                    long rCount = cRightArray != null ? cRightArray[s] : count - s;

                    var cBox = Box.Intersection(lBox, rBox);
                    var cSize = new V3d(cBox.Max.X > cBox.Min.X ? cBox.Max.X - cBox.Min.X : 0,
                                        cBox.Max.Y > cBox.Min.Y ? cBox.Max.Y - cBox.Min.Y : 0,
                                        cBox.Max.Z > cBox.Min.Z ? cBox.Max.Z - cBox.Min.Z : 0);
                    var commonP = 2 * (cSize.X * cSize.Y + cSize.X * cSize.Z + cSize.Y * cSize.Z)
                                    * invBoxArea;

                    var cost = (leftP + (commonP * c_splitPenalty)) * (1 + lCount)
                              + (rightP + (commonP * c_splitPenalty)) * (1 + rCount);

                    if (cost < bestCost)
                    {
                        bestCost = cost; bestDim = d; bestLeft = s;
                        bestLeftBox = lBox;  bestRightBox = rBox;
                    }
                }
            }
            sortedIndexArray = iaa[bestDim];
            leftBox = bestLeftBox;
            rightBox = bestRightBox;
            return bestLeft;
        }

        private int CreateLeaf(InParams inParams, int[] indices, int signedCount)
        {
            var index = inParams.LeafCount;
            if (signedCount > 0)
            {
                m_leafArray[index++] = signedCount;
                indices.CopyTo(m_leafArray, index);
                inParams.LeafCount = index + signedCount;
            }
            else
            {
                m_leafArray[index++] = -signedCount;
                indices.CopyTo(indices.Length + signedCount, -signedCount, m_leafArray, index);
                inParams.LeafCount = index - signedCount;
            }
            return -index;
        }

        private int Create(InParams inParams, int[] indices, int start, int count, Box3d box)
        {
            int ni = inParams.NodeCount++;
            int[] sortedIndices;
            Box3d leftBox, rightBox;
            var leftCount = CalculateSplit(inParams, indices, start, count, box,
                                           out sortedIndices, out leftBox, out rightBox);
            var rightCount = count - leftCount;

            m_indexArray[2 * ni] = leftCount > inParams.LeafLimit
                ? Create(inParams, sortedIndices, 0, leftCount, leftBox)
                : m_leafArray == null ? -1 - sortedIndices[0] : CreateLeaf(inParams, sortedIndices, leftCount);
            m_indexArray[2 * ni + 1] = rightCount > inParams.LeafLimit
                ? Create(inParams, sortedIndices, leftCount, rightCount, rightBox)
                : m_leafArray == null ? -1 - sortedIndices[count - 1] : CreateLeaf(inParams, sortedIndices, -rightCount);

            if ((inParams.BuildFlags & BuildFlags.CreateBoxArrays) != 0)
            {
                m_leftBoxArray[ni] = leftBox;
                m_rightBoxArray[ni] = rightBox;
            }
            if ((inParams.BuildFlags & BuildFlags.CreateCombinedArray) != 0)
                m_combinedBoxArray[ni] = new Box3dAndFlags(box, leftBox, rightBox);
            return ni;
        }

        #endregion

    }
}