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

            //~UnitOfWork()
            //{
            //    Dispose(disposing: false);
            //}

            #region Protected Methods

            protected override void Dispose(bool disposing)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        // _contactRepository.dis
                    }

                    _disposed = true;
                }
                // Call the base class implementation if this class is inherited.
                base.Dispose(disposing);
            }

            #endregion
    }
}
