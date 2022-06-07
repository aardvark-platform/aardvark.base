namespace Aardvark.Base.Windows.Tests

module Program =

    open BenchmarkDotNet.Running;
    open BenchmarkDotNet.Configs
    open BenchmarkDotNet.Jobs
    open BenchmarkDotNet.Toolchains

    [<EntryPoint>]
    let main argv =

        //let cfg =
        //    let job = Job.ShortRun.WithToolchain(InProcess.Emit.InProcessEmitToolchain.Instance)
        //    ManualConfig.Create(DefaultConfig.Instance).WithOptions(ConfigOptions.DisableOptimizationsValidator).AddJob(job)

        //BenchmarkSwitcher.FromAssembly(typeof<ZipFloatArrays>.Assembly).Run(argv, cfg) |> ignore;
        0