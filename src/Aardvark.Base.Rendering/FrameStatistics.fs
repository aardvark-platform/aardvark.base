namespace Aardvark.Base

type FrameStatistics =
    {
        Procedures : float
        InstructionCount : float
        ActiveInstructionCount : float
        DrawCallCount : float
        ResourceUpdateCount : float
        PrimitiveCount : float
        JumpDistance : float
        AddedRenderJobs : float
        RemovedRenderJobs : float
    } with

    static member Zero =
        {
            Procedures = 0.0
            InstructionCount = 0.0
            ActiveInstructionCount = 0.0
            DrawCallCount = 0.0
            ResourceUpdateCount = 0.0
            PrimitiveCount = 0.0
            JumpDistance = 0.0
            AddedRenderJobs = 0.0
            RemovedRenderJobs = 0.0
        }

    static member DivideByInt(l : FrameStatistics, r : int) =
        l / float r

    static member (+) (l : FrameStatistics, r : FrameStatistics) =
        {
            Procedures = l.Procedures + r.Procedures
            InstructionCount = l.InstructionCount + r.InstructionCount
            ActiveInstructionCount = l.ActiveInstructionCount + r.ActiveInstructionCount
            DrawCallCount = l.DrawCallCount + r.DrawCallCount
            ResourceUpdateCount = l.ResourceUpdateCount + r.ResourceUpdateCount
            PrimitiveCount = l.PrimitiveCount + r.PrimitiveCount
            JumpDistance = l.JumpDistance + r.JumpDistance
            AddedRenderJobs = l.AddedRenderJobs + r.AddedRenderJobs
            RemovedRenderJobs = l.RemovedRenderJobs + r.RemovedRenderJobs
        }

    static member (-) (l : FrameStatistics, r : FrameStatistics) =
        {
            Procedures = l.Procedures - r.Procedures
            InstructionCount = l.InstructionCount - r.InstructionCount
            ActiveInstructionCount = l.ActiveInstructionCount - r.ActiveInstructionCount
            DrawCallCount = l.DrawCallCount - r.DrawCallCount
            ResourceUpdateCount = l.ResourceUpdateCount - r.ResourceUpdateCount
            PrimitiveCount = l.PrimitiveCount - r.PrimitiveCount
            JumpDistance = l.JumpDistance - r.JumpDistance
            AddedRenderJobs = l.AddedRenderJobs - r.AddedRenderJobs
            RemovedRenderJobs = l.RemovedRenderJobs - r.RemovedRenderJobs
        }

    static member (/) (l : FrameStatistics, r : float) =
        {
            Procedures = l.Procedures / r
            InstructionCount = l.InstructionCount / r
            ActiveInstructionCount = l.ActiveInstructionCount / r
            DrawCallCount = l.DrawCallCount / r
            ResourceUpdateCount = l.ResourceUpdateCount / r
            PrimitiveCount = l.PrimitiveCount / r
            JumpDistance = l.JumpDistance / r
            AddedRenderJobs = l.AddedRenderJobs / r
            RemovedRenderJobs = l.RemovedRenderJobs / r
        }


