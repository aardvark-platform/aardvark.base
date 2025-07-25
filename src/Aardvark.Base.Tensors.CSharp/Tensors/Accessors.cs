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
using System;
using System.Collections.Generic;

namespace Aardvark.Base;

public interface ITensorAccessors
{
}

public class TensorAccessors<Td, Tv> : ITensorAccessors
{
    public Func<Td[], long, Tv> Getter;
    public Action<Td[], long, Tv> Setter;
}

public static partial class TensorAccessors
{
    public static class Intent
    {
        public static readonly Symbol BW = Col.Intent.BW;
        public static readonly Symbol Gray = Col.Intent.Gray;
        public static readonly Symbol Alpha = Col.Intent.Alpha;

        public static readonly Symbol RGB = Col.Intent.RGB;
        public static readonly Symbol BGR = Col.Intent.BGR;
        public static readonly Symbol RGBA = Col.Intent.RGBA;
        public static readonly Symbol BGRA = Col.Intent.BGRA;

        public static readonly Symbol ColorChannel = "ColorChannel";
    };

    public static TensorAccessors<Td, Tv> Get<Td, Tv>(Symbol intent, long[] delta)
    {
        Type dataType = typeof(Td);
        Type viewType = typeof(Tv);

        if (s_creatorMap.TryGetValue((dataType, viewType, intent), out var creator))
        {
            if (creator(delta) is TensorAccessors<Td, Tv> typed) return typed;
        }
        throw new KeyNotFoundException(
            $"No accessors to view {dataType} as {viewType} with intent {intent}."
        );
    }
}
