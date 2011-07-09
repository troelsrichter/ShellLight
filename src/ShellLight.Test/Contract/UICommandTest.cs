using NUnit.Framework;
using ShellLight.Test.Commands;

namespace ShellLight.Test.Contract
{
    [TestFixture]
    public class UICommandTest
    {
        [Test]
        public void IconSourceConventionTest()
        {
            var command = new IconConventionCommand();
            Assert.AreEqual("/ShellLight.Test;component/Icons/IconConvention.png",command.IconSource,"should return custom icon source");

            var command2 = new IconConventionDefaultImageCommand();
            Assert.AreEqual("/ShellLight;component/Images/default.png",command2.IconSource,"should return default icon source");
        }

        [Test]
        public void NameConventionTest()
        {
            var command = new IconConventionCommand();
            Assert.AreEqual("Icon Convention", command.Name, "should return conventional name");
        }
    }
}