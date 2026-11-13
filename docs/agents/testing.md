# Testing that earns its cost

The feedback loop for a small edit takes seconds of source review, not a suite
run per task. Choose evidence for the failure the change could cause.
[Building](building.md) owns when to build and how to run the tests;
[AGENTS.md](../../AGENTS.md#working-rules) governs permission to run tests and
open windows.

## The everyday loop

Choose evidence for each distinct failure the change could cause. Use source
reasoning, compiler guarantees and existing coverage first; run the smallest
existing check that adds evidence, with its `--filter`. Two checks of the same
behavior need a distinct failure each can catch. A full suite is for a
deliberate release check or an explicit owner request. Documentation edits need
no build and no tests.

Include reflection-based test calls when reviewing consumers before a build:
several tests reach internal members by name, so a rename the compiler accepts
can still break them. After a finding, review the correction and rerun only the
checks whose evidence it changes.

## Test only what changed

**Owner ruling: run checks only on behavior the change touched.** A check of
unchanged behavior cannot fail because of the change, so it costs minutes of
the owner's shared laptop and finds nothing. Run only the test methods whose
subject is the changed code path, and report which ran and what each covered.

## Verification must answer a question

Before a test run or a window opens, the message names the open question and
the result that would change the next step.

These are settled already, so they are never open questions: the compiler
accepted the code; a known control shows the property the code sets;
documented platform behavior; behavior the change did not touch; and an answer
a later correction cannot change. A typical open question is uncertain
clipping at a size, an interaction between controls the code cannot predict,
or motion the person sees. General authorization to validate a change is not
an open question; source review completes the work when none remains.

## What earns a test

**Owner ruling: high-return tests only.** Review and measurement are the
safety net, not tests. A test that does not earn its place is deleted later, so
writing it is work done twice.

**One question decides it: what specific failure does this test watch for?**
Name what breaks, what a host or a person would observe, and why nothing else
already catches it: an existing test, the compiler or WinUI. When there is no
answer, there is nothing to test: say so in the report. An instruction to "add
tests" is bound by the same question.

Failures worth watching:

- Lost or wrong selection, current item or focus across a source change, a
  sort or a keyed replacement.
- A restored layout snapshot that saved data from an earlier version no longer
  reads.
- A rule of the contract, pinned once: one test of the rule beats twelve rows
  of examples.
- A defect already paid for that a plausible simplification would bring back.
- A rule that nothing else guards.

**Presentation is reviewed, never asserted.** Text, wording, XAML, colors,
spacing and screenshots change with every copy or layout edit while the
behavior stands still. A measured requirement, such as a contrast ratio, is a
contract, not presentation.

Report each new or materially expanded test method with the failure it
watches, what would be observed, and why existing coverage misses it.

## A test asserts the outcome the failure would change

Assert what the host or person would see go wrong: the selection, the order of
the rows, the layout snapshot, the event a gesture raised, the state a control
is left in. The expected value is a literal the scenario predicts, never a
value the production code computes again. A test that still passes when the
feature is deleted watches nothing. A test runs the code; it never reads a
source file.

## The cheapest layer, the smallest set

Prove the failure at the cheapest layer that can fail for that reason: a
loaded table driven through its API before real injected input. Pin a rule
with one representative case, plus one case for each boundary that is
materially different.

A test is deterministic or it is a defect. Fix the source of the
nondeterminism, or delete the test; a retry, a sleep or a longer timeout hides
it. Wait for observable completion, with a bound. Reuse the existing harnesses
(`TableHarness`, `SelectionHarness`, `ProofHarness`, `TestData`); production
keeps its shape, so no member is made public only for a test.

## A failing test has three outcomes

1. Production is wrong: fix production.
2. The test pinned behavior that changed on purpose: delete or rewrite it, and
   report what it pinned.
3. The test was wrong: fix it and say why.

Decide which before you touch either. Weakening an assertion, widening a
tolerance, copying the observed value into the expected one, raising a timeout
or skipping the test is none of the three. When you change a string, member
name or resource key, search for the literal: a test's class name does not say
what it asserts.

## Interactive tests

The tests in category `Interactive` inject real mouse and keyboard input. They
take the owner's pointer and keyboard for the whole run, and the owner's own
input makes them fail. Run them only with the owner's agreement, while the
machine is idle, and only after a change to input handling, a template part
name or an event subscription: the three ways the input path breaks silently.
[Building](building.md#run-the-tests) shows how every other run keeps them out.

## Evidence from windows

The sample and the test host open real windows on the owner's desktop. Report
which kind of evidence you have: source review, compilation, automated checks
or observation, and name none of them as stronger than it is. A compile result,
an in-process automation cache or an old measurement cannot establish current
pixels, Narrator behavior or responsiveness.

Judge motion live. Paint metrics from a test host have reported a row slide as
working while the owner saw a shuffle. Before a change to row motion is
accepted, the owner watches both builds side by side in the sample.
