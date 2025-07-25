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
#if !NET8_0_OR_GREATER

using System.ComponentModel;

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace System.Runtime.CompilerServices;

// See: https://github.com/dotnet/roslyn/issues/45510
[EditorBrowsable(EditorBrowsableState.Never)]
public static class IsExternalInit
{
}

#endif