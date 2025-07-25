/*
    Copyright 2006-2025. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
namespace Aardvark.Base;

/// <summary>
/// An editable face.
/// </summary>
public interface IEditableFace2d :
    IBehaviorPosition2d,
    IBehaviorTransform2d,
    IBehaviorDeletable
{
    /// <summary>
    /// Returns index-th vertex as editable vertex.
    /// </summary>
    IEditableVertex2d GetEditableVertex(int index);

    /// <summary>
    /// Returns index-th edge as editable edge.
    /// </summary>
    IEditableEdge2d GetEditableEdge(int index);
}
