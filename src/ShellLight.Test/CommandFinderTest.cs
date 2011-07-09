using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using NUnit.Framework;
using ShellLight.Contract;
using ShellLight.Contract.Attributes;

namespace ShellLight.Test
{
    [TestFixture]
    public class CommandFinderTest
    {
        [Test]
        public void FindShouldUseWildcardSearch()
        {
            var commands = new List<UICommand> {new CreateUserCommand(), new DeleteUserCommand(), new CreateTaskCommand()};
            string outParameter = null;
            var resultCommands = CommandFinder.Find("create", commands, out outParameter);
            Assert.AreEqual(2,resultCommands.Count);
        }

        [Test]
        public void FindShouldExtractParameterFromSearchText()
        {
            var commands = new List<UICommand> {new CreateUserCommand(), new CreateTaskCommand()};
            string outParameter = null;
            var resultCommands = CommandFinder.Find("create user gimufafi", commands, out outParameter);
            Assert.AreEqual(1, resultCommands.Count, "should be only one create user command");
            Assert.AreEqual("gimufafi", outParameter, "should extract parameter from search text");
        }

        [Test]
        public void FindShouldExtractParameterFromSearchTextWithWildcard()
        {
            var commands = new List<UICommand> { new CreateUserCommand(), new CreateTaskCommand() };
            string outParameter = null;
            var resultCommands = CommandFinder.Find("create u gimufafi", commands, out outParameter);
            Assert.AreEqual(1, resultCommands.Count, "should be only one create user command");
            Assert.AreEqual("gimufafi", outParameter, "should extract parameter from search text");
        }

        [Test]
        public void FilterCommandsShouldNotIncludeHiddenHiddenCommads()
        {
            var commands = new ObservableCollection<UICommand> { new CreateUserCommand(), new CreateTaskCommand(), new HiddenCommand() };
            var resultCommands = CommandFinder.FilterCommands(commands);
            Assert.AreEqual(2, resultCommands.Count);
            var hiddenCommands = from c in resultCommands where c is HiddenCommand select c;
            Assert.AreEqual(0,hiddenCommands.Count(),"there should be no hidden commands");
        }
        
    }

    public class CreateUserCommand: UICommand
    {
        public override UIElement DoShow()
        {
            throw new NotImplementedException();
        }
    }

    public class DeleteUserCommand : UICommand
    {
        public override UIElement DoShow()
        {
            throw new NotImplementedException();
        }
    }


    public class CreateTaskCommand: UICommand
    {
        public override UIElement DoShow()
        {
            throw new NotImplementedException();
        }
    }

    [Launcher(VisibilityType.Hidden)]
    public class HiddenCommand: UICommand
    {
        public override UIElement DoShow()
        {
            throw new NotImplementedException();
        }
    }
}