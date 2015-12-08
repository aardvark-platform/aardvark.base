module Yampa =
    type SF<'s, 'a, 'b> = { run : 's -> 'a -> 's * 'b }

    module SF =
        let arr (f : 'a -> 'b) =
            { run = fun s a -> s,f a }

        let compose(g : SF<'s, 'b, 'c>) (f : SF<'s, 'a, 'b>) =
            { run = fun s a ->
                let (s,b) = f.run s a
                g.run s b
            }

        let first (f : SF<'s, 'a, 'b>) : SF<'s, 'x * 'a, 'x * 'b> =
            { run = fun s (x,a) ->
                let (s,b) = f.run s a
                (s, (x,b))
            }

        let second (f : SF<'s, 'a, 'b>) : SF<'s, 'a * 'x, 'b * 'x> =
            { run = fun s (a,x) ->
                let (s,b) = f.run s a
                (s, (b,x))
            }

        let map (f : 'b -> 'c) (m : SF<'s, 'a, 'b>) =
            { run = fun s v ->
                let (s,r) = m.run s v
                (s, f r)
            }

        let par (f : SF<'s, 'a, 'b>) (g : SF<'s, 'a, 'c>) =
            { run = fun s v ->
                let (s, b) = f.run s v
                let (s, c) = g.run s v

                (s, (b,c))

            }

        let zip (f : SF<'s, 'a, 'b>) (g : SF<'s, 'x, 'y>) =
            { run = fun s (a,x) ->
                let (s,b) = f.run s a
                let (s,y) = g.run s x
                (s,(b,y))
            }
