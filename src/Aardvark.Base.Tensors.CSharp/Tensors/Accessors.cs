using System;
using System.Collections.Generic;

namespace Aardvark.Base
{
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

}
