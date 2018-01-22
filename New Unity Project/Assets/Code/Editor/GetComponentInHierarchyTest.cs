using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace TankGame.Testing
{
    public class GetComponentInHierarchyTest
    {

        private GameObject _parent;
        private GameObject _child;
        private GameObject _grandChild;

        private GetComponentInHierarchyTester Setup
            (bool includeInactive, bool componentInParent, bool setActive)
        {
            _parent = new GameObject();
            _child = new GameObject();
            _grandChild = new GameObject();

            _child.transform.parent = _parent.transform;
            _grandChild.transform.parent = _child.transform;

            GetComponentInHierarchyTester tester =
                _child.AddComponent<GetComponentInHierarchyTester>();

            tester.SetUp(includeInactive, componentInParent, setActive);

            return tester;
        }

        [Test]
        public void GetComponentInHierarchyTest_ComponentInChild_IncludeInactive_SetActive()
        {
            GetComponentInHierarchyTester tester = Setup(true, false, true);
            TestComponent result = tester.Run();
            Assert.NotNull(result);
        }

        [Test]
        public void GetComponentInHierarchyTest_ComponentInChild_DontIncludeInactive_SetActive()
        {
            GetComponentInHierarchyTester tester = Setup(false, false, true);
            TestComponent result = tester.Run();
            Assert.NotNull(result);
        }

        [Test]
        public void GetComponentInHierarchyTest_ComponentInChild_IncludeInactive_SetInactive()
        {
            GetComponentInHierarchyTester tester = Setup(true, false, false);
            TestComponent result = tester.Run();
            Assert.NotNull(result);
        }

        [Test]
        public void GetComponentInHierarchyTest_ComponentInChild_DontIncludeInactive_SetInactive()
        {
            GetComponentInHierarchyTester tester = Setup(false, false, false);
            TestComponent result = tester.Run();
            Assert.IsNull(result);
        }

        [Test]
        public void GetComponentInHierarchyTest_ComponentInParet_IncludeInactive_SetActive()
        {
            GetComponentInHierarchyTester tester = Setup(true, true, true);
            TestComponent result = tester.Run();
            Assert.NotNull(result);
        }

        [Test]
        public void GetComponentInHierarchyTest_ComponentInParet_DontIncludeInactive_SetActive()
        {
            GetComponentInHierarchyTester tester = Setup(false, true, true);
            TestComponent result = tester.Run();
            Assert.NotNull(result);
        }

        [Test]
        public void GetComponentInHierarchyTest_ComponentInParet_IncludeInactive_SetInactive()
        {
            GetComponentInHierarchyTester tester = Setup(true, true, false);
            TestComponent result = tester.Run();
            Assert.NotNull(result);
        }


        [Test]
        public void GetComponentInHierarchyTest_ComponentInParet_DontIncludeInactive_SetInactive()
        {
            GetComponentInHierarchyTester tester = Setup(false, true, false);
            TestComponent result = tester.Run();
            Assert.IsNull(result);
        }
    }
}
