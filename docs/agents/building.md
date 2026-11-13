# Building and review

A WinUI build takes minutes on the owner's 15 W laptop and slows everything else
on it, and several agents share that machine. These rules keep builds rare, one
at a time, and used.

## Compile last

**Owner ruling: delay compilation until nothing else can answer the
question.** A build started early is repeated after the next edit.

1. Finish all the requested code and the
   [focused review passes](#focused-review-passes).
2. Answer every question you can without a build: read the code, its callers
   and the platform documentation. An existing output shows the code before
   your edits; it cannot verify them. Documentation-only changes need no build.
3. List the questions that still need a build. Each must pass the
   [verification gate](testing.md#verification-must-answer-a-question).
4. Build once, for that whole list, and run every check on its output.

When no remaining question needs a build, source review completes verification.

## Build

Build and run tests only when the owner asks
([working rules](../../AGENTS.md#working-rules)). Otherwise end the work at
source review and name what only a build or a test run could settle. Run one build at a time: check first that no other build of this
repository is running, because two builds write the same `artifacts\` folder.

From the repository root, in PowerShell 7, build the project a check needs:

```powershell
dotnet build tests\TableViewTests.csproj -c Debug -p:Platform=x64
dotnet build sample\TableViewSample.csproj -c Debug -p:Platform=x64
```

`TableView.slnx` builds the library, the tests and the sample together; use it
only when a check needs all three. Output goes to `artifacts\` (a junction to
`V:\artifacts\TableView`), never inside a project folder.

A host that includes this repository builds it through its own build script.
TinyTorrent links this repository at `lib\TableView` and builds it with
`Build.ps1 -Project lib\TableView\tests\TableViewTests.csproj`; that script
waits for other builds, reuses current output and lowers the build's priority.

Keep [Directory.Build.props](../../Directory.Build.props)'s generated-output
exclusions: they stop the XAML compiler's `bin-fl` output from re-entering
source globs. If you change them, verify that repeated builds do not deepen the
output tree.

## Run the tests

`dotnet test` on the project applies [tests/.runsettings](../../tests/.runsettings),
which excludes the input-injecting `Interactive` tests. Run on the built DLL,
the settings file is not applied, so always add the filter:

```powershell
dotnet test artifacts\bin\TableViewTests\debug_win-x64\TableViewTests.dll --filter "TestCategory!=Interactive&FullyQualifiedName~SelectionTests"
```

Narrow the filter to the changed behavior, as [testing](testing.md) requires.
**Owner ruling: a test run ends when its tests end.** A test process that stays
after its tests finish has failed, whatever it reported: it holds the laptop
and can lock files the next build needs. Fix the cause before other work.

## When a build fails

Read the whole log and fix every error before you build again.

- Errors in your change: fix them.
- Errors in files another agent is changing: leave its files alone, report the
  task as blocked with the log path, and build again only after those files
  change.
- The same error after your fix: stop and report the error, what you changed
  and the log path.

## Focused review passes

Finish the feature and review it before you compile. Each pass reads the
current result of the preceding fixes.

1. **Correctness and paths.** Trace every changed behavior from its entry points
   through its callers, state changes and consumers, including reflection in
   the tests. Follow empty, error, cancellation, repeated-operation and lifetime
   paths. Complete when every affected path is traced and every finding has a
   fix or an explicit unresolved reason.
2. **Ownership and simplicity.** Apply the
   [code style](../../AGENTS.md#code-style) to the feature as a whole: each rule
   has one owner and one implementation, with no duplication, competing state
   or ceremony without a requirement. Complete when every remaining layer and
   piece of state has a concrete purpose.
3. **Fresh read after fixes.** Read the result as its next maintainer. Recheck
   the paths the corrections touched, then apply the [naming](naming.md) and
   [comment](comments.md) policies. Complete when no actionable finding
   remains unaccounted for.

A new finding means repair and another pass over the affected behavior. Scale
the passes to the change: a small fix gets three short passes, not three
reports.
