namespace ICSSoft.STORMNET.Tests.TestClasses.Common
{
    using System;
    using System.Collections;

    using ICSSoft.STORMNET.KeyGen;

    using Xunit;

    
    public class CommonTest
    {
        [Fact]
        public void ArrayAccessor()
        {
            ArrayList arl = new ArrayList();
            arl.Add(5);
            Assert.NotNull(arl[0]);
            object o = arl[0];
            arl[0] = null;
            Console.WriteLine(o);
            Assert.NotNull(o);
        }

        [Fact]
        public void KeyGuidCompare()
        {
            KeyGuid kg1 = new KeyGuid("{01666FC6-4A58-4DB0-BE32-D47533F1E64A}");
            KeyGuid kg2 = new KeyGuid("{01666FC6-4A58-4DB0-BE32-D47533F1E64A}");

            Assert.True(kg1.Equals(kg2));
            Assert.True(kg1 == "{01666FC6-4A58-4DB0-BE32-D47533F1E64A}");
            Guid guid = new Guid();
            Assert.False((Guid)kg1 == guid);

        }

    }
}
