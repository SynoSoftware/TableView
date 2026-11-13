# TableView

TableView is a reusable WinUI 3 table control for large, changing collections
with rich cells. The library is in [src](src/), its focused tests in
[tests](tests/), and a demonstration host with diagnostic probes in
[sample](sample/). [TableView.slnx](TableView.slnx) opens all three. TinyTorrent
is the main host: it links this repository at its `lib\TableView`, so an edit
here reaches TinyTorrent at once and is committed here, not there.

## First directive: common sense above all

**Owner ruling.** Use judgment to achieve the user's actual goal with the least
unnecessary effort. Every rule and plan here serves that outcome; apply it by
its purpose, not mechanically. Resolve routine decisions within the
authorization already given. Ask when an unresolved choice materially changes
scope, risk or authorization. Match implementation, review and validation
effort to the change, because process that costs more than it protects delays
delivery.

## Every rule states why

A rule in these instructions or in the contract states what it protects, in the
present tense. Without its reason, nobody can tell whether the rule still
applies, so it is either copied into cases it was never about or dropped when it
is inconvenient. How a rule arose belongs in the commit message.

Before applying a rule, check that its reason holds in the case in front of you.
If it does not, apply the first directive and explain the departure. A rule whose
reason you cannot find is a finding.

A rule marked **Owner ruling** is the owner's settled decision. Reviews, plans
and later agents build on it as fixed. When a case seems to conflict with one,
follow the ruling and report the conflict to the owner, because a ruling that
each new agent reopens changes the control's direction from session to session.

## Read for the change

- Before changing behavior or the public API, read the relevant sections of the
  [contract](docs/tableview-contract.md), in the [table vocabulary](CONTEXT.md).
- Before changing appearance, read the
  [visual contract](docs/tableview-contract.md#8-rendering-layout-and-visual-language).
  The library owns table design, so every host shows the same control.
- Before changing state ownership, input, rendering or layout, read the
  [implementation map](docs/tableview-implementation.md).
- Before changing hierarchical rows, read the [hierarchy extension](docs/hierarchy.md).
- Before designing or changing a type, an enum, an interface or where a seam
  goes, use the `/codebase-design` skill, so every session judges a design by
  one standard.
- Before naming or placing a type, member, file, folder, namespace or resource
  key, read [naming](docs/agents/naming.md).
- Before adding, changing or reviewing a comment, read
  [comments](docs/agents/comments.md).
- Before writing, changing, choosing or running tests, read
  [testing](docs/agents/testing.md).
- Before compiling, running a host, or finishing an implementation, read
  [building](docs/agents/building.md). It holds the review passes.

[Archived documents](docs/archive/README.md) record earlier designs and
measurements; they are history, not a second plan.

## Control work

Preserve the public and observable contract. A simpler mechanism may replace an
existing one, but a perceived flaw in the contract is a finding to resolve
before changing behavior. Change the specification only on purpose, never to
match a regression.

Keep the control independent of any host's types, commands, persistence and
preferences. The host owns domain policy; TableView owns the mechanics its
contract describes. Use the existing non-torrent sample to check that new
behavior belongs in the library, rather than inventing another host.

**Owner ruling: a TableView change starts from the table's history.** Before
designing a change, read every commit since the `v0.9.1.2` release
(`git log v0.9.1.2..HEAD`) and the documents, `docs/archive` included. Name
each earlier attempt at the same change or its opposite: what it did, why its
commit message says it ended, and what the new design does better. The table's
behavior has gone back and forth: the 3 November 2026 revert to `v0.9.1.2` was
made for memory and also removed two owner rulings on switching to a different
list, and the next commit built the opposite behavior.
[WinUI list behavior the design depends on](docs/tableview-implementation.md#winui-list-behavior-the-design-depends-on)
lists the platform facts those attempts found. The `v…` tags are TinyTorrent
releases: each marks the table that shipped in that release.

The performance goal is the result, in the owner's words: "all I want is this
to be as fast as possible", at real sizes (normally 100 to 500 rows, sometimes
5,000, rarely 10,000), with WinUI's own row animations kept and memory not
growing. Retained memory and code size are costs to cut, and a mechanism stays
only with a measured gain at real sizes.

Inspect the current code before acting on historical findings. Keep shared
geometry, selection, source projection and gesture state at the owners the
implementation map names. A menu action and its gesture call the same
operation.

Acknowledge input promptly while expensive work proceeds. Preserve the
deliberate press and release decisions in contract sections 14 and 16; the time
a person holds an input is not evidence of a slow handler.

Use the platform value when Windows owns a threshold, theme metric or behavior;
use the effective layout for geometric values and the contract for specified
baselines. Verify resource names against the installed platform rather than
inventing them. The table's resource constraints and their reasons live in its
contract.

Search all string-bound consumers when changing template parts, visual states,
resource keys or automation identifiers: typed reference searches cannot find
those contracts, and the tests reach several internal members by name. Keep
live XAML bindings live; `x:Bind` defaults to one-time.

## Text and languages

**Owner ruling: keep all shipped languages current.** The control's text lives
in [src/Resources](src/Resources/): `en.json` and one catalogue per shipped
language. When a message changes, update it in every catalogue with the same
placeholders, so every person reads the same information in their language.
`Strings` falls back through parent languages to English for a missing
message. The host prepares `Strings` and owns its language setting; contract
section 6.1 states how a language change reaches the table.

## Working rules

- **Owner ruling: filename discovery.** Find files with
  `C:\Program Files\gozler\gozler.exe`, with the narrowest `-path` and a
  specific name or pattern. A search that includes `artifacts` adds
  `"!*\artifacts\*"`, because `artifacts` is a link to `V:` and holds build
  output. Search file contents with `rg`.
- **Owner ruling: user steers are cumulative.** Keep earlier requirements and
  unfinished work in scope unless the user cancels or replaces them, and do
  them in the order received.
- **Owner ruling: no redundant verification; every check answers an open
  question.** [Testing](docs/agents/testing.md#verification-must-answer-a-question)
  says what counts. Each check costs minutes of the owner's shared laptop.
- **Owner ruling: tests are tripwires, not coverage.** Write a test only to
  catch a regression someone relies on;
  [testing](docs/agents/testing.md#what-earns-a-test) decides.
- **Owner ruling: "no build, no tests. unless i ask".** A build or test run
  costs minutes of the shared laptop, and the owner decides when one is worth
  it. [Building](docs/agents/building.md) says how, and how a test run ends.
- Before reporting work complete, do the
  [focused review passes](docs/agents/building.md#focused-review-passes).
- State assumptions and verifiable success criteria before implementing.
- Leave line endings alone. `.gitattributes` stores text as LF, so the CRLF
  that Windows tools write never reaches a diff. A line-ending change that does
  show in `git diff` is a defect in that conversion: report it.

## Code style

The test for every change: an engineer new to the feature finds what an
operation does, where its rules live, and how its data changes by reading the
code, without a map of the architecture.

1. **Correctness first.** Simplicity never justifies weaker behavior. Work that
   makes the person wait, or that the table repeats without need, is weaker
   behavior, so its cost is decided while the code is designed.
   - **Work follows the change.** Code that runs for every update, frame,
     input or row does work sized by what changed or by what is on screen,
     because work sized by the whole list grows with the list. Work of that
     size stays off the UI thread and out of a frame.
   - **Owner ruling: every stage that redoes work an earlier stage already
     did, or makes an object only to drop it, is fixed; the collector is the
     evidence, not the target.** WinUI answers memory growth with a blocking
     collection that stops every thread that runs .NET code.
2. **Clarity over cleverness.** Code is read and changed far more often than it
   is written.
   - Choose algorithms and data structures by complexity at the real scale,
     preferring proven library implementations; among equally effective ones,
     choose the easiest to understand.
   - **Choose the right model for the behavior.** Booleans hold independent
     facts, enums mutually exclusive states, state machines constrained
     transitions, and polymorphism behavior that differs by implementation.
     Interacting flags and repeated conditions signal a wrong model.
   - Requests, results and errors that cross the API have named types.
   - Make invalid states unrepresentable where the type system can. Give each
     state its own value, so no empty string, zero or invented sentinel
     selects a second behavior. Use the null-forgiving `!` only where a real
     invariant guarantees the value.
   - Dependencies, state changes, side effects and failures show at the call.
3. **One responsibility, one owner, one implementation.** Two copies of a rule
   drift.
   - Commands and gestures for the same operation call the same
     implementation.
   - Owners meet at a small boundary, and the owner that knows a fact passes
     it, such as what changed, so the next owner does not work it out again.
     Count the work of a path as a whole, across every owner on it.
   - Validation has one owner where data enters, such as the host's schema at
     capture or a restored layout; code inside trusts the result.
   - Store facts and decisions; derive display values when read.
4. **Minimal architecture that works.** Every layer, interface, abstraction and
   file earns its place by removing more complexity than it adds. Use C#, .NET
   and WinUI directly, the way their documentation shows. Build for the
   requirements that exist now.
5. **Improve without expanding scope.** Follow the existing solution to the
   same problem; usability wins when it conflicts with consistency. When the
   feature you change holds the worse solution, restructure it in the same
   change. Look first for what to remove, and when a new implementation
   replaces an old one, remove the old one in the same change. Outside the
   feature, remove only orphans the change creates and report other problems.
