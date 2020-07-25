This is a Recursive Descent Parser for a restricted form of SQL

The lexical syntax is specified by regular expressions:

| Category        | Definition  |
| ------------- | ------------- |
| digit      | [0-9] |
| letter      | [a-zA-Z]      |
| int | <digit>+      |
| float | <digit>+.<digit>+      |
| id | <letter>(<letter> j <digit>)*      |
| keyword | SELECT \| FROM \| WHERE \| AND |
| operator | = \| < \| > |
| comma | , |


The concrete syntax is specified by the following `EBNF` grammar, where `<Query>` is the start symbol:
```
<Query> -> SELECT <IdList> FROM <IdList> [WHERE <CondList>]

<IdList> -> <id> {, <id>}

<CondList> -> <Cond> {AND | OR <Cond>}

<Cond> -> <id> <operator> <Term>

<Term> -> <id> | <int> | <float>

```

Here **\<id>**, **\<float>**, and **\<operator>** are token categories defined by the regular expressions above,
and the terminal symbols **SELECT**, **FROM**, **WHERE**, and **AND** are of category keyword, while "**,**"
is a terminal symbol with category **\<comma>**.
Note arbitrary whitespace can appear between tokens, and no space is needed between an operator and its operands,
or between the items in a list and the commas that separate them.