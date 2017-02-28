namespace Aardvark.Base

open System

type Monoid<'a> =
    {
        misEmpty    : 'a -> bool
        mempty      : 'a
        mappend     : 'a -> 'a -> 'a
    }
