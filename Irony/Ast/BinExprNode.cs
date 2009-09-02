﻿#region License
/* **********************************************************************************
 * Copyright (c) Roman Ivantsov
 * This source code is subject to terms and conditions of the MIT License
 * for Irony. A copy of the license can be found in the License.txt file
 * at the root of this distribution. 
 * By using this source code in any fashion, you are agreeing to be bound by the terms of the 
 * MIT License.
 * You must not remove this notice from this software.
 * **********************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Interpreter;
using Irony.Parsing;

namespace Irony.Ast {
  public class BinExprNode : AstNode {
    public AstNode Left;
    public string Op;
    public AstNode Right;

    public BinExprNode() { }
    public override void Init(ParsingContext context, ParseTreeNode treeNode) {
      base.Init(context, treeNode);
      Left = AddChild("Arg", treeNode.ChildNodes[0]);
      Right = AddChild("Arg", treeNode.ChildNodes[2]);
      Op = treeNode.ChildNodes[1].FindTokenAndGetText(); 
      AsString = Op + "(operator)"; 
    }

    public override void Evaluate(EvaluationContext context, AstMode mode) {
      Left.Evaluate(context, AstMode.Read);
      Right.Evaluate(context, AstMode.Read);
      try {
        context.CallDispatcher.ExecuteBinaryOperator(this.Op);
      } catch (DivideByZeroException ex) {
        throw new RuntimeException(ex.Message, ex, this.Span.Location); 
      }
    }

  }//class
}//namespace
