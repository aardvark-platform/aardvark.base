namespace Aardvark.Rendering

/// Represents a moving average window of a sequence.
/// It builds the average of the last N inserted values.
type AverageWindow(maxCount : int) =
    let values = Array.zeroCreate maxCount
    let mutable index = 0
    let mutable count = 0
    let mutable sum = 0.0

    /// Insert a new value to the sequence and returns the average of the last N values.
    member x.Insert(v : float) =
        let newSum =
            if count < maxCount then
                count <- count + 1
                sum + v
            else
                sum + v - values.[index]

        sum <- newSum
        values.[index] <- v
        index <- (index + 1) % maxCount

        x.Value

    /// The number of currently inserted values.
    member x.Count = count

    /// Returns the average of the last N inserted values.
    member x.Value =
        if count = 0 then 0.0
        else sum / float count
