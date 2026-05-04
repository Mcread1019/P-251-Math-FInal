using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LogicGame
{
    public static class QuestionBank
    {
        // ── TFL Questions (20) ──────────────────────────────────────────────────
        public static readonly List<MultipleChoiceQuestion> TFLQuestions = new List<MultipleChoiceQuestion>
        {
            // ── Easy (7) ──
            new MultipleChoiceQuestion(
                "Which TFL sentence is a tautology?",
                new[]{"P & ~P", "P v ~P", "P -> Q", "P <-> ~P"},
                1,
                "P v ~P is true on every valuation — every sentence is either true or false.",
                Difficulty.Easy, QuestionType.TFL),

            new MultipleChoiceQuestion(
                "A TFL sentence is a contradiction iff:",
                new[]{"It is true on every valuation",
                      "It is false on every valuation",
                      "It is true on exactly one valuation",
                      "It contains only atomic sentences"},
                1,
                "A contradiction is false under every possible truth-value assignment.",
                Difficulty.Easy, QuestionType.TFL),

            new MultipleChoiceQuestion(
                "A -> B is FALSE when:",
                new[]{"A is false and B is true",
                      "A is true and B is true",
                      "A is true and B is false",
                      "A is false and B is false"},
                2,
                "A conditional is false only when the antecedent is true and the consequent is false.",
                Difficulty.Easy, QuestionType.TFL),

            new MultipleChoiceQuestion(
                "Which is logically equivalent to ~(A & B)?",
                new[]{"~A & ~B", "~A v ~B", "A v ~B", "~A -> B"},
                1,
                "De Morgan's law: the negation of a conjunction is the disjunction of the negations.",
                Difficulty.Easy, QuestionType.TFL),

            new MultipleChoiceQuestion(
                "An argument is TFL-valid iff:",
                new[]{"The conclusion is a tautology",
                      "No valuation makes all premises true and the conclusion false",
                      "All premises are tautologies",
                      "There exists a valuation making every sentence true"},
                1,
                "Validity means truth-preservation: no counter-example valuation exists.",
                Difficulty.Easy, QuestionType.TFL),

            new MultipleChoiceQuestion(
                "A <-> B is TRUE when:",
                new[]{"A is true and B is false",
                      "A is false and B is true",
                      "A and B have opposite truth values",
                      "A and B have the same truth value"},
                3,
                "A biconditional is true exactly when both sides share the same truth value.",
                Difficulty.Easy, QuestionType.TFL),

            new MultipleChoiceQuestion(
                "A & B is true iff:",
                new[]{"At least one of A or B is true",
                      "Both A and B are true",
                      "A is true",
                      "B is true"},
                1,
                "Conjunction requires both conjuncts to be true.",
                Difficulty.Easy, QuestionType.TFL),

            // ── Medium (7) ──
            new MultipleChoiceQuestion(
                "Which argument form is INVALID?",
                new[]{"P -> Q,  P  |-  Q  (modus ponens)",
                      "P -> Q,  ~Q  |-  ~P  (modus tollens)",
                      "P -> Q,  Q  |-  P  (affirming the consequent)",
                      "P v Q,  ~P  |-  Q  (disjunctive syllogism)"},
                2,
                "Affirming the consequent is a fallacy: Q being true doesn't force P to be true.",
                Difficulty.Medium, QuestionType.TFL),

            new MultipleChoiceQuestion(
                "Which set of connectives is functionally complete?",
                new[]{"{&}", "{v}", "{&, ~}", "{->}"},
                2,
                "{&, ~} is complete: v can be defined as ~(~A & ~B). Neither {&} nor {v} alone is complete.",
                Difficulty.Medium, QuestionType.TFL),

            new MultipleChoiceQuestion(
                "The Sheffer stroke (A nand B) is FALSE only when:",
                new[]{"Both A and B are true",
                      "A is true and B is false",
                      "A is false and B is true",
                      "Both A and B are false"},
                0,
                "A nand B is false precisely when both inputs are true (it is NOT-AND).",
                Difficulty.Medium, QuestionType.TFL),

            new MultipleChoiceQuestion(
                "If A is a tautology, what can be said about A v B for any B?",
                new[]{"It is always false",
                      "It is a tautology",
                      "It has the same truth value as B",
                      "It is false whenever B is false"},
                1,
                "If A is always true, then A v B is always true regardless of B.",
                Difficulty.Medium, QuestionType.TFL),

            new MultipleChoiceQuestion(
                "~(A v B) is logically equivalent to:",
                new[]{"~A v ~B", "~A & ~B", "A & ~B", "~A -> B"},
                1,
                "De Morgan's law: the negation of a disjunction is the conjunction of the negations.",
                Difficulty.Medium, QuestionType.TFL),

            new MultipleChoiceQuestion(
                "A == B (logical equivalence) means:",
                new[]{"A -> B is a tautology",
                      "A and B have the same truth value on every valuation",
                      "A and B contain the same atomic sentences",
                      "A and B are both tautologies"},
                1,
                "Logical equivalence holds when both sentences always agree in truth value.",
                Difficulty.Medium, QuestionType.TFL),

            new MultipleChoiceQuestion(
                "TFL is:",
                new[]{"Sound but not complete",
                      "Complete but not sound",
                      "Both sound and complete",
                      "Neither sound nor complete"},
                2,
                "TFL's proof system is both sound (|- implies |=) and complete (|= implies |-).",
                Difficulty.Medium, QuestionType.TFL),

            // ── Hard (6) ──
            new MultipleChoiceQuestion(
                "A -> (B -> C) is logically equivalent to:",
                new[]{"(A -> B) -> C", "(A & B) -> C", "A -> (B & C)", "(A v B) -> C"},
                1,
                "Exportation: A -> (B -> C) == (A & B) -> C.",
                Difficulty.Hard, QuestionType.TFL),

            new MultipleChoiceQuestion(
                "Which is NOT logically equivalent to P -> Q?",
                new[]{"~P v Q", "~Q -> ~P", "~(P & ~Q)", "P v ~Q"},
                3,
                "P -> Q == ~P v Q. But P v ~Q == ~P -> ~Q, which differs (e.g., P=T, Q=F makes P->Q false but P v ~Q true).",
                Difficulty.Hard, QuestionType.TFL),

            new MultipleChoiceQuestion(
                "Which set of connectives is functionally complete on its own?",
                new[]{"{->, <->}", "{v, &}", "{nand}", "{->}"},
                2,
                "The Sheffer stroke {nand} alone is complete: ~A == A nand A, and A & B == (A nand B) nand (A nand B).",
                Difficulty.Hard, QuestionType.TFL),

            new MultipleChoiceQuestion(
                "Which is a valid TFL argument?",
                new[]{"P v Q,  P  |-  ~Q",
                      "P -> Q,  Q -> R  |-  P -> R",
                      "P -> Q,  R -> Q  |-  P -> R",
                      "P,  Q  |-  P <-> Q"},
                1,
                "Hypothetical syllogism (P->Q, Q->R |- P->R) is valid. The others have counter-examples.",
                Difficulty.Hard, QuestionType.TFL),

            new MultipleChoiceQuestion(
                "(P -> Q) -> ((Q -> R) -> (P -> R)) is:",
                new[]{"A contradiction", "Contingent", "A tautology", "Logically equivalent to P -> R"},
                2,
                "This is the hypothetical syllogism tautology — true on every valuation.",
                Difficulty.Hard, QuestionType.TFL),

            new MultipleChoiceQuestion(
                "G |= C means: (G stands for the set of premises Gamma)",
                new[]{"There is a formal proof of C from G",
                      "Every valuation satisfying all of G also satisfies C",
                      "C is a tautology whenever G is satisfiable",
                      "There exists a valuation making all of G and C true"},
                1,
                "|= denotes semantic consequence: truth-preservation across all valuations.",
                Difficulty.Hard, QuestionType.TFL),
        };

        // ── FOL Questions (20) ──────────────────────────────────────────────────
        public static readonly List<MultipleChoiceQuestion> FOLQuestions = new List<MultipleChoiceQuestion>
        {
            // ── Easy (7) ──
            new MultipleChoiceQuestion(
                "\"All Fs are Gs\" is best symbolized in FOL as:",
                new[]{"Ex(Fx & Gx)", "Ax(Fx -> Gx)", "Ax(Fx & Gx)", "Ex(Fx -> Gx)"},
                1,
                "Universal generalizations use ->: Ax(Fx -> Gx). Using & would say everything is both F and G.",
                Difficulty.Easy, QuestionType.FOL),

            new MultipleChoiceQuestion(
                "In FOL, the existential quantifier (written Ex) means:",
                new[]{"For all x", "There exists an x such that", "For no x", "For exactly one x"},
                1,
                "Ex is the existential quantifier — it asserts that at least one object satisfies the formula.",
                Difficulty.Easy, QuestionType.FOL),

            new MultipleChoiceQuestion(
                "The domain of quantification must:",
                new[]{"Contain at least one object",
                      "Contain infinitely many objects",
                      "Only include objects named by constants",
                      "Be the set of all real numbers"},
                0,
                "The domain must be non-empty; objects need not be named, and it can be any non-empty set.",
                Difficulty.Easy, QuestionType.FOL),

            new MultipleChoiceQuestion(
                "ExFx is logically equivalent to:",
                new[]{"AxFx", "~Ax~Fx", "Ax~Fx", "~Ex~Fx"},
                1,
                "ExFx == ~Ax~Fx: 'something is F' is the same as 'not everything fails to be F'.",
                Difficulty.Easy, QuestionType.FOL),

            new MultipleChoiceQuestion(
                "\"Karl loves someone\" (Lxy: x loves y, k: Karl) is symbolized as:",
                new[]{"AxLkx", "Lkk", "ExLkx", "AxLxk"},
                2,
                "The existential quantifier captures 'someone': ExLkx says there is some x that Karl loves.",
                Difficulty.Easy, QuestionType.FOL),

            new MultipleChoiceQuestion(
                "An FOL interpretation must specify:",
                new[]{"Only a domain",
                      "A domain and predicate meanings",
                      "A domain, name references, and predicate extensions",
                      "Only the meanings of predicates"},
                2,
                "A full interpretation gives: domain, reference for each name, and extension for each predicate/relation.",
                Difficulty.Easy, QuestionType.FOL),

            new MultipleChoiceQuestion(
                "In FOL, \"a = b\" is true in an interpretation iff:",
                new[]{"a and b share all the same properties",
                      "a and b name the very same object in the domain",
                      "a and b are syntactically identical constants",
                      "The domain has exactly one object"},
                1,
                "Identity is checked extensionally: a = b is true iff both names refer to the exact same domain object.",
                Difficulty.Easy, QuestionType.FOL),

            // ── Medium (7) ──
            new MultipleChoiceQuestion(
                "What is the difference between Ex(Ax & Bx) and (ExAx & ExBx)?",
                new[]{"They are logically equivalent",
                      "The first requires one object to satisfy both; the second allows different objects for each",
                      "The first uses universal quantification",
                      "The second requires one object to satisfy both"},
                1,
                "Ex(Ax & Bx) says one thing is both A and B. ExAx & ExBx allows separate witnesses for A and B.",
                Difficulty.Medium, QuestionType.FOL),

            new MultipleChoiceQuestion(
                "\"Karl loves someone who loves themselves\" (k: Karl, Lxy: x loves y) is:",
                new[]{"Ax(Lkx & Lxx)", "Ex(Lkx & Lxx)", "ExLkx & ExLxx", "Ex(Lkx -> Lxx)"},
                1,
                "Ex(Lkx & Lxx): there is one x such that Karl loves x AND x loves x.",
                Difficulty.Medium, QuestionType.FOL),

            new MultipleChoiceQuestion(
                "Domain = {1,2,3}, F = {1,2}, G = {2,3}. Is Ex(Fx & Gx) true?",
                new[]{"No, because F and G are different sets",
                      "Yes, because 2 is in both F and G",
                      "No, because F and G overlap only partially",
                      "Cannot determine without identity statements"},
                1,
                "2 is in F and 2 is in G, so object 2 witnesses the existential.",
                Difficulty.Medium, QuestionType.FOL),

            new MultipleChoiceQuestion(
                "An interpretation specifies extensions rather than meanings. This reflects:",
                new[]{"The compositionality principle",
                      "The extensionality principle",
                      "The soundness principle",
                      "The completeness principle"},
                1,
                "Extensionality: truth values depend only on which objects fall under predicates, not on what those predicates 'mean'.",
                Difficulty.Medium, QuestionType.FOL),

            new MultipleChoiceQuestion(
                "AxFx is logically equivalent to:",
                new[]{"ExFx", "~Ex~Fx", "Ex~Fx", "~Ax~Fx"},
                1,
                "AxFx == ~Ex~Fx: 'everything is F' means 'there is no thing that fails to be F'.",
                Difficulty.Medium, QuestionType.FOL),

            new MultipleChoiceQuestion(
                "Domain = {a,b}, H = {a,b} (H: is happy). Is AxHx true?",
                new[]{"No, we need to check objects outside the domain",
                      "Yes, because every object in the domain is in H",
                      "Only if a = b",
                      "Cannot be determined"},
                1,
                "AxHx is true iff every domain object is in H's extension. Both a and b are in {a,b}.",
                Difficulty.Medium, QuestionType.FOL),

            new MultipleChoiceQuestion(
                "\"If Karl loves himself then he loves everyone\" (k: Karl, Lxy: x loves y):",
                new[]{"Lkk & AxLkx", "Lkk -> AxLkx", "Ax(Lkx -> Lkk)", "ExLkx -> Lkk"},
                1,
                "A conditional whose antecedent is Lkk and whose consequent universally quantifies: Lkk -> AxLkx.",
                Difficulty.Medium, QuestionType.FOL),

            // ── Hard (6) ──
            new MultipleChoiceQuestion(
                "Which interpretation makes AxEyLxy TRUE but EyAxLxy FALSE?",
                new[]{"Domain={a}, L={(a,a)}",
                      "Domain={a,b}, L={(a,b), (b,a)}",
                      "Domain={a,b}, L={}",
                      "Domain={a,b}, L={(a,a),(a,b),(b,a),(b,b)}"},
                1,
                "In {a,b} with L={(a,b),(b,a)}: everyone loves someone (true), but no one is loved by everyone (false).",
                Difficulty.Hard, QuestionType.FOL),

            new MultipleChoiceQuestion(
                "\"Exactly one thing is F\" is best expressed in FOL as:",
                new[]{"ExFx",
                      "AxFx",
                      "Ex(Fx & Ay(Fy -> x=y))",
                      "ExEy(Fx & Fy)"},
                2,
                "Russell's uniqueness clause: something is F, and anything that is F is identical to it.",
                Difficulty.Hard, QuestionType.FOL),

            new MultipleChoiceQuestion(
                "Domain={1,2,3}, L={(1,2),(2,3),(3,1)}. Is AxEyLxy true?",
                new[]{"No, because 1 does not love itself",
                      "Yes, because every object loves some object",
                      "Only if the domain contains more objects",
                      "Cannot determine"},
                1,
                "1 loves 2, 2 loves 3, 3 loves 1 — every object has at least one thing it loves.",
                Difficulty.Hard, QuestionType.FOL),

            new MultipleChoiceQuestion(
                "\"The F is G\" (Russell: Ex(Fx & Ay(Fy -> x=y) & Gx)) is true iff:",
                new[]{"At least one F is G",
                      "All Fs are G",
                      "Exactly one thing is F, and it is G",
                      "There is a name that refers to the unique F"},
                2,
                "Russell's theory requires existence, uniqueness, and satisfaction: one and only one F, and that F is G.",
                Difficulty.Hard, QuestionType.FOL),

            new MultipleChoiceQuestion(
                "Domain={k,j}, L={(k,j)}, k=Karl, j=John.\nEx(Lkx & Ay~Lxy) — is this true?",
                new[]{"False, because k loves j",
                      "True, because k loves j and j loves nobody",
                      "True, because k does not love himself",
                      "False, because j might love someone outside the domain"},
                1,
                "Witness x=j: Lkj is true, and j has no outgoing loves (Ay~Ljy is true). So the existential holds.",
                Difficulty.Hard, QuestionType.FOL),

            new MultipleChoiceQuestion(
                "ExEy(x!=y & Fx & Fy) expresses:",
                new[]{"Exactly one thing is F",
                      "No thing is F",
                      "At least two distinct things are F",
                      "All things are F"},
                2,
                "The formula witnesses two distinct objects x and y both satisfying F.",
                Difficulty.Hard, QuestionType.FOL),
        };

        // ── Soundness / Shininess Fill-in-Blank Questions (20) ─────────────────
        // Notation used: δ=delta, φ=phi (Greek letters render fine).
        // Subscripts use plain letters: δi = delta_i, δn = delta_n, etc.
        // Operators: |- for turnstile, |= for semantic consequence, C= for subset-eq.
        public static readonly List<FillBlankQuestion> SoundnessQuestions = new List<FillBlankQuestion>
        {
            // ── Easy (7) — 1 blank each ──
            new FillBlankQuestion(
                "A line i in a proof is shiny iff [_0] |= φi.",
                new[]{"δi"},
                new[]{"δi", "φi", "G", "v"},
                "Shininess means the sentence on line i is semantically entailed by the assumptions in force at that line.",
                Difficulty.Easy),

            new FillBlankQuestion(
                "The Soundness Theorem states: If G |- C, then G [_0] C.",
                new[]{"|="},
                new[]{"|=", "|-", "==", "->"},
                "|= is semantic consequence. The theorem says provability implies logical consequence.",
                Difficulty.Easy),

            new FillBlankQuestion(
                "The Shininess Lemma states that [_0] line in every TFL proof is shiny.",
                new[]{"every"},
                new[]{"every", "some", "no", "the last"},
                "Every single line — not just the last — must be shiny. This is what the inductive proof establishes.",
                Difficulty.Easy),

            new FillBlankQuestion(
                "Rule PR is rule-sound because φn is an element of [_0], so any valuation making it true also makes φn true.",
                new[]{"δn"},
                new[]{"δn", "δi", "G", "φn"},
                "A premise is among its own assumptions δn, so it trivially entails itself.",
                Difficulty.Easy),

            new FillBlankQuestion(
                "A proof system is [_0] iff whenever G |- C, we have G |= C.",
                new[]{"sound"},
                new[]{"sound", "complete", "valid", "shiny"},
                "Soundness means the proof system never proves invalid arguments.",
                Difficulty.Easy),

            new FillBlankQuestion(
                "δi includes premises and assumptions of all [_0] subproofs at line i.",
                new[]{"open"},
                new[]{"open", "closed", "nested", "discharged"},
                "Once a subproof closes its assumption is discharged and leaves δ. Only open subproof assumptions remain.",
                Difficulty.Easy),

            new FillBlankQuestion(
                "In the Horseshoe Method, Step 1 involves considering an arbitrary valuation v that satisfies [_0].",
                new[]{"δn"},
                new[]{"δn", "δi", "G", "φn"},
                "We start at bottom-left: pick any v satisfying all the assumptions at the line we want to prove shiny.",
                Difficulty.Easy),

            // ── Medium (7) — 2-3 blanks each ──
            new FillBlankQuestion(
                "A line i is shiny iff [_0] |= [_1].",
                new[]{"δi", "φi"},
                new[]{"δi", "φi", "δn", "φn", "G", "C"},
                "Shininess: the set of active assumptions (δi) semantically entails the sentence on that line (φi).",
                Difficulty.Medium),

            new FillBlankQuestion(
                "Since closed subproofs cannot be cited, for any line i cited by line n we have [_0] C= [_1].",
                new[]{"δi", "δn"},
                new[]{"δi", "δn", "δj", "G", "φi", "φn"},
                "Citing a line requires it to be in scope, which means its assumptions are a subset of the current line's assumptions.",
                Difficulty.Medium),

            new FillBlankQuestion(
                "For &E: line n cites line i where φi = φn [_0] B. By the inductive hypothesis, [_1] |= φi.",
                new[]{"&", "δi"},
                new[]{"&", "v", "->", "<->", "δi", "δn", "G"},
                "&E eliminates a conjunction: φi must have & as its main connective. The IH gives us shininess of line i.",
                Difficulty.Medium),

            new FillBlankQuestion(
                "Shininess is like [_0] applied to individual lines, rather than to whole [_1].",
                new[]{"validity", "arguments"},
                new[]{"validity", "soundness", "arguments", "proofs", "tautologies", "sentences"},
                "Each line implicitly asserts δi therefore φi, which is legitimate iff that mini-argument is valid.",
                Difficulty.Medium),

            new FillBlankQuestion(
                "A rule R is [_0]-sound if applying it to shiny lines always yields a [_1] line.",
                new[]{"rule", "shiny"},
                new[]{"rule", "proof", "shiny", "valid", "sound", "true"},
                "Rule-soundness means the rule is shininess-preserving — it can't break the inductive property.",
                Difficulty.Medium),

            new FillBlankQuestion(
                "δi is the set of [_0] and [_1] under which line i is asserted.",
                new[]{"premises", "assumptions"},
                new[]{"premises", "assumptions", "conclusions", "lines", "rules", "valuations"},
                "δi collects the global premises plus any assumptions introduced by still-open subproofs.",
                Difficulty.Medium),

            new FillBlankQuestion(
                "The Horseshoe Method has [_0] steps. In Step [_1] we use δi C= δn to move from bottom-left to top-left.",
                new[]{"4", "2"},
                new[]{"4", "2", "3", "5", "1", "0"},
                "The four steps trace a horseshoe path: schema (0), assume v satisfies δn (1), subset step (2), apply IH and truth table (3).",
                Difficulty.Medium),

            // ── Hard (6) — 4-5 blanks each ──
            new FillBlankQuestion(
                "PR rule-soundness: Line n cites PR, so φn is in [_0]. Any valuation [_1] all of [_2] trivially makes [_3] true. So [_4] |= φn.",
                new[]{"δn", "satisfying", "δn", "φn", "δn"},
                new[]{"δn", "δi", "satisfying", "falsifying", "φn", "φi", "G"},
                "A premise is in its own δ, so any valuation making all of δ true automatically makes the premise true.",
                Difficulty.Hard),

            new FillBlankQuestion(
                "vI rule-soundness: φn = φi [_0] B. By IH, [_1] |= [_2]. Since δi C= δn, any v satisfying δn satisfies δi, so v makes [_3] true. By the truth table for [_4], v makes φn true.",
                new[]{"v", "δi", "φi", "φi", "v"},
                new[]{"v", "&", "->", "δi", "δn", "φi", "φn", "G"},
                "vI introduces a disjunction: if φi is true and φi v B has φi as a disjunct, the disjunction is true.",
                Difficulty.Hard),

            new FillBlankQuestion(
                "Soundness from Shininess: The [_0] line of the proof has sentence [_1] and assumptions [_2]. The Shininess Lemma gives [_3] |= [_4], which is exactly G |= C.",
                new[]{"last", "C", "G", "G", "C"},
                new[]{"last", "first", "C", "φn", "G", "δn", "v"},
                "The final line's sentence is the conclusion C, and by the time we reach it all subproofs have closed, leaving only G (Gamma).",
                Difficulty.Hard),

            new FillBlankQuestion(
                "&I rule-soundness: φn = [_0] & [_1], citing lines i and j. By IH δi |= [_2] and δj |= [_3]. Since both δi and [_4] are subsets of δn, any v satisfying δn makes both conjuncts true.",
                new[]{"φi", "φj", "φi", "φj", "δj"},
                new[]{"φi", "φj", "φn", "δi", "δj", "δn", "G", "v"},
                "&I builds a conjunction from two lines; both must be shiny, and both assumption sets sit inside the current δn.",
                Difficulty.Hard),

            new FillBlankQuestion(
                "AS rule-soundness: φn is an [_0] opening a subproof. It appears in [_1] (it is among the active assumptions), so any v making [_2] true makes [_3] true. Therefore [_4] |= φn.",
                new[]{"assumption", "δn", "δn", "φn", "δn"},
                new[]{"assumption", "premise", "δn", "δi", "φn", "φi", "G", "v"},
                "An assumption is placed in δ the moment it opens a subproof, so it trivially entails itself.",
                Difficulty.Hard),

            new FillBlankQuestion(
                "A formal proof system is sound iff: whenever [_0] |- [_1], then [_2] |= [_3]. The symbol [_4] is read 'proves' or 'derives'.",
                new[]{"G", "C", "G", "C", "|-"},
                new[]{"G", "C", "δi", "φi", "|-", "|=", "v", "R"},
                "Sound means every provable argument is also a valid argument — derivability implies semantic consequence.",
                Difficulty.Hard),
        };

        // ── Accessors ───────────────────────────────────────────────────────────

        public static List<MultipleChoiceQuestion> GetMCQByDifficulty(Difficulty d)
        {
            var combined = new List<MultipleChoiceQuestion>(TFLQuestions);
            combined.AddRange(FOLQuestions);
            return combined.Where(q => q.difficulty == d).ToList();
        }

        public static List<FillBlankQuestion> GetSoundnessByDifficulty(Difficulty d)
            => SoundnessQuestions.Where(q => q.difficulty == d).ToList();

        public static List<MultipleChoiceQuestion> GetRandomMCQ(Difficulty d, int count)
        {
            var pool = GetMCQByDifficulty(d);
            Shuffle(pool);
            return pool.Take(Mathf.Min(count, pool.Count)).ToList();
        }

        public static FillBlankQuestion GetRandomSoundness(Difficulty d)
        {
            var pool = GetSoundnessByDifficulty(d);
            if (pool.Count == 0) return null;
            return pool[Random.Range(0, pool.Count)];
        }

        static void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
