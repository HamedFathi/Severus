using Severus;
using System;
using System.Collections.Generic;
using System.Text;



namespace ParserSample
{
    public class SqlParser : ParserWand<SqlTokenType, Query>
    {
        public SqlParser(LexerResult<SqlTokenType> lexerResult) : base(lexerResult)
        {

        }

        public override Query Parse()
        {
            var output = QueryExpression();
            return output;
        }

        private Query QueryExpression()
        {
            var q = new Query();

            // SELECT
            //if (Peek().Type == SqlTokenType.Keyword && Peek().Value.ToLower() == SqlKeyword.SELECT.GetDescription().ToLower())
            if (IsMatchType(SqlTokenType.Keyword) && IsMatchValue(SqlKeyword.SELECT.GetDescription()))
            {
                // Read(() => { return result; });
                var status = Read();
                if (!status) return q;

                var output = IdListExpression();
                q.Select = output;
            }
            else
            {
                q.AddError(Error(Peek(), SqlKeyword.SELECT.GetDescription()));
            }

            // FROM
            if (Peek().Type == SqlTokenType.Keyword && Peek().Value.ToLower() == SqlKeyword.FROM.GetDescription().ToLower())
            {
                var status = Read();
                if (!status) return q;

                var output = IdListExpression();
                q.From = output;

            }
            else
            {
                q.AddError(Error(Peek(), SqlKeyword.FROM.GetDescription()));
            }

            // WHERE
            if (Peek().Type == SqlTokenType.Keyword && Peek().Value.ToLower() == SqlKeyword.WHERE.GetDescription().ToLower())
            {
                var status = Read();
                if (!status) return q;

                var output = CondListExpression();
                q.Where = output;

            }
            else
            {
                q.AddError(Error(Peek(), SqlKeyword.WHERE.GetDescription()));
            }
            return q;
        }

        /*private ParserResult<SqlQuery> CondListExpression()
        {
            throw new NotImplementedException();
        }*/

        private IdList IdListExpression()
        {
            var l = new IdList();

            if (Peek().Type == SqlTokenType.Id)
            {
                var outputId = Peek().Value;
                l.Ids.Add(outputId);
                var statusId = Read();
                if (!statusId) return l;
            }
            else
            {
                l.AddError(Error(Peek(), SqlTokenType.Id.GetDescription()));
            }
            while (Peek().Type == SqlTokenType.Comma)
            {
                var outputComma = Peek().Value;

                var statusComma = Read();
                if (!statusComma) return l;

                if (Peek().Type == SqlTokenType.Id)
                {
                    var outputIdList = Peek().Value;
                    l.Ids.Add(outputIdList);

                    var statusIdList = Read();
                    if (!statusIdList) return l;
                }
                else
                {
                    l.AddError(Error(Peek(), SqlTokenType.Id.GetDescription()));
                }
            }
            return l;
        }

        private ConditionList CondListExpression()
        {
            var c = new ConditionList();

            if (Peek().Type == SqlTokenType.Id)
            {
                var output = CondExpression();
                c.Conditions.Add(new ConditionItem() { PreOperator = "", Condition = output });
            }
            else
            {
                c.AddError(Error(Peek(), SqlTokenType.Id.GetDescription()));
            }

            if (Peek().Type == SqlTokenType.Keyword)
            {
                while (!IsEndOfInput() && Peek().Type == SqlTokenType.Keyword &&
                (Peek().Value.ToLower() == SqlKeyword.AND.GetDescription().ToLower()
                    || Peek().Value.ToLower() == SqlKeyword.OR.GetDescription().ToLower()))
                {
                    var outputOperator = Peek().Value;

                    var statusIdList = Read();
                    if (!statusIdList) return c;

                    if (Peek().Type == SqlTokenType.Id)
                    {
                        var outputCond = CondExpression();
                        c.Conditions.Add(new ConditionItem() { PreOperator = outputOperator, Condition = outputCond });
                    }
                    else
                    {
                        c.AddError(Error(Peek(), SqlTokenType.Id.GetDescription()));
                    }

                }
            }
            else
            {
                c.AddError(Error(Peek(), "AND or OR"));
            }
            return c;

        }

        private Condition CondExpression()
        {
            var c = new Condition();

            if (Peek().Type == SqlTokenType.Id)
            {
                var outputId = Peek().Value;
                c.Id = outputId;

                var statusId = Read();
                if (!statusId) return c;

                if (Peek().Type == SqlTokenType.Operator)
                {
                    var outputOperator = Peek().Value;
                    c.Operator = outputOperator;

                    var statusOperator = Read();
                    if (!statusOperator) return c;

                    var outputTerm = TermExpression();
                    c.Term = outputTerm;
                }
                else
                {
                    c.AddError(Error(Peek(), SqlTokenType.Operator.GetDescription()));
                }
            }
            else
            {
                c.AddError(Error(Peek(), SqlTokenType.Id.GetDescription()));
            }

            return c;

        }

        private Term TermExpression()
        {
            var t = new Term();
            var token = Peek();
            t.Value = token.Value;
            switch (token.Type)
            {
                case SqlTokenType.Int:
                case SqlTokenType.Float:
                case SqlTokenType.Id:
                    t.Type = token.Type;
                    Read();
                    break;
                default:
                    t.AddError(Error(Peek(), SqlTokenType.Id.GetDescription()));
                    break;
            }
            return t;
        }
    }
}
