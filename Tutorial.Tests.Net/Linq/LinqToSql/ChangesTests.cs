﻿namespace Dixin.Tests.Linq.LinqToSql
{
    using System;
    using System.Diagnostics;
    using System.Transactions;
    using Dixin.Linq.LinqToSql;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ChangesTests
    {
        [TestMethod]
        public void TracingTest()
        {
            Tracking.EntitiesFromSameDataContext();
            Tracking.ObjectsFromSameDataContext();
            Tracking.EntitiesFromDataContexts();
            Tracking.EntityChanges();
            Tracking.Attach();
            Tracking.AssociationChanges();
            try
            {
                Tracking.DisableObjectTracking();
                Assert.Fail();
            }
            catch (InvalidOperationException exception)
            {
                Trace.WriteLine(exception);
            }
        }

        [TestMethod]
        public void ChangesTest()
        {
            using (TransactionScope scope = new TransactionScope())
            {
                int subcategoryId = Changes.Create();
                Changes.Update();
                Changes.UpdateWithNoChange();
                Changes.Delete();
                Changes.DeleteWithNoQuery(subcategoryId);
                Changes.DeleteWithAssociation();
                try
                {
                    Changes.UntrackedChanges();
                    Assert.Fail();
                }
                catch (InvalidOperationException exception)
                {
                    Trace.WriteLine(exception);
                }
                scope.Complete();
            }
        }
    }
}