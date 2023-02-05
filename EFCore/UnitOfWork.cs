using ApiWithUnitTetsting.Context;

namespace ApiWithUnitTetsting.EFCore
{
        public class UnitOfWork : EFCoreUnitOfWorkBase
        {
            #region Private Fields

            private bool _disposed = false;

            #endregion

            #region Public Constructors

            public UnitOfWork() : base(new UnitTestingContext())
            { }

            #endregion


            #region Protected Methods

            protected override void Dispose(bool disposing)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                    }

                    _disposed = true;
                }
                base.Dispose(disposing);
            }

            #endregion
    }
}
