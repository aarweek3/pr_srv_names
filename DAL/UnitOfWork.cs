// DAL/UnitOfWork.cs - ������ ����������

using DAL.Interfaces;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using DAL.Repositories.Interfaces.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL
{
    /// <summary>
    /// ������ ���������� Unit of Work ��������
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Dictionary<Type, object> _repositories = new();
        private bool _disposed = false;
        private IDbContextTransaction? _currentTransaction;

        // ReSharper disable once ConvertToPrimaryConstructor
        public UnitOfWork(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // ������ � UnitOfWork ��� ���� � ��������:

        private INameMainRepository? _nameMainRepository;
        public INameMainRepository NameMains => _nameMainRepository ??= new NameMainRepository(_context);

        private IAnecdoteRepository? _anecdoteRepository;
        public IAnecdoteRepository Anecdotes => _anecdoteRepository ??= new AnecdoteRepository(_context);

        private ILanguageRepository? _languageRepository;
        public ILanguageRepository Languages => _languageRepository ??= new LanguageRepository(_context);


        private INameRepository? _nameRepository;
        public INameRepository Names => _nameRepository ??= new NameRepository(_context);

        private INameDescriptionRepository? _nameDescriptionRepository;

        public INameDescriptionRepository NameDescriptions =>
            _nameDescriptionRepository ??= new NameDescriptionRepository(_context);

        private ISeoDataRepository? _seoDataRepository;
        public ISeoDataRepository SeoDatas => _seoDataRepository ??= new SeoDataRepository(_context);

        private ISampleRepository? _sampleRepository;
        public ISampleRepository Samples => _sampleRepository ??= new SampleRepository(_context);

        // ===========================
        // �������� ���������
        // ===========================
        public DbContext Context => _context;

        // ===========================
        // ������������������ ����������� � lazy loading
        // ===========================
        private IUserRepository? _userRepository;
        public IUserRepository Users => _userRepository ??= new UserRepository(_context);

        private IUserSessionRepository? _userSessionRepository;
        public IUserSessionRepository UserSessions => _userSessionRepository ??= new UserSessionRepository(_context);

        private IActivityLogRepository? _activityLogRepository;
        private IUserSettingsRepository? _userSettingsRepository;
        public IActivityLogRepository ActivityLogs => _activityLogRepository ??= new ActivityLogRepository(_context);
        public IUserSettingsRepository UserSettings => _userSettingsRepository ??= new UserSettingsRepository(_context);

        // ===========================
        // GENERIC REPOSITORY
        // ===========================
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories.TryGetValue(typeof(TEntity), out var repository))
                return (IRepository<TEntity>)repository;

            var newRepo = new Repository<TEntity>(_context);
            _repositories.Add(typeof(TEntity), newRepo);
            return newRepo;
        }

        // ===========================
        // ���������� ���������
        // ===========================
        public int SaveChanges()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new InvalidOperationException("�������� ������������� ���������� ��� ���������� ���������.", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("������ ��� ���������� ��������� � ���� ������.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("����������� ������ ��� ���������� ���������.", ex);
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new InvalidOperationException("�������� ������������� ���������� ��� ���������� ���������.", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException("������ ��� ���������� ��������� � ���� ������.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("����������� ������ ��� ���������� ���������.", ex);
            }
        }

        // ===========================
        // ���������� - ���������� ������
        // ===========================
        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null)
                throw new InvalidOperationException(
                    "���������� ��� ������. ��������� ������� ���������� ����� ������� �����.");

            _currentTransaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("��� �������� ���������� ��� �������������.");

            try
            {
                await _currentTransaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await RollbackTransactionAsync();
                throw new InvalidOperationException("������ ��� ������������� ����������.", ex);
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("��� �������� ���������� ��� ������.");

            try
            {
                await _currentTransaction.RollbackAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("������ ��� ������ ����������.", ex);
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        // ===========================
        // ���������� - ����������� ������
        // ===========================
        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Database.BeginTransactionAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("������ ��� ������ ����������.", ex);
            }
        }

        public async Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            using var transaction = await BeginTransactionAsync(cancellationToken);
            try
            {
                await action();
                await SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action,
            CancellationToken cancellationToken = default)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            using var transaction = await BeginTransactionAsync(cancellationToken);
            try
            {
                var result = await action();
                await SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return result;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        // ===========================
        // SQL �������
        // ===========================
        public async Task<int> ExecuteSqlRawAsync(string sql, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new ArgumentException("SQL ������ �� ����� ���� ������.", nameof(sql));

            try
            {
                return await _context.Database.ExecuteSqlRawAsync(sql, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"������ ��� ���������� SQL �������: {sql}", ex);
            }
        }

        public async Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new ArgumentException("SQL ������ �� ����� ���� ������.", nameof(sql));

            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            try
            {
                return await _context.Database.ExecuteSqlRawAsync(sql, parameters);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"������ ��� ���������� SQL ������� � �����������: {sql}", ex);
            }
        }

        // ===========================
        // ���������� ����������
        // ===========================
        public void DetachAllEntities()
        {
            try
            {
                _context.ChangeTracker.Clear();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("������ ��� ������������ ��������� �� ���������.", ex);
            }
        }

        public async Task ResetSequenceAsync<TEntity>() where TEntity : class
        {
            try
            {
                var entityType = _context.Model.FindEntityType(typeof(TEntity));
                if (entityType == null)
                    throw new InvalidOperationException($"��� �������� {typeof(TEntity).Name} �� ������ � ������.");

                var tableName = entityType.GetTableName();
                var schemaName = entityType.GetSchema() ?? "public";

                if (string.IsNullOrEmpty(tableName))
                    throw new InvalidOperationException(
                        $"�� ������� �������� ��� ������� ��� ���� {typeof(TEntity).Name}.");

                // PostgreSQL ������ ������ ������������������
                var sequenceName = $"{tableName}_id_seq";
                var sql = $"ALTER SEQUENCE {schemaName}.{sequenceName} RESTART WITH 1";

                await ExecuteSqlRawAsync(sql);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"������ ��� ������ ������������������ ��� {typeof(TEntity).Name}.",
                    ex);
            }
        }

        public bool HasUnsavedChanges()
        {
            try
            {
                return _context.ChangeTracker.HasChanges();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("������ ��� �������� ������������� ���������.", ex);
            }
        }

        public void RejectChanges()
        {
            try
            {
                foreach (var entry in _context.ChangeTracker.Entries())
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entry.CurrentValues.SetValues(entry.OriginalValues);
                            entry.State = EntityState.Unchanged;
                            break;
                        case EntityState.Added:
                            entry.State = EntityState.Detached;
                            break;
                        case EntityState.Deleted:
                            entry.State = EntityState.Unchanged;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("������ ��� ������ ���������.", ex);
            }
        }

        // ===========================
        // �������������� ������
        // ===========================
        public async Task<bool> CanConnectAsync()
        {
            try
            {
                return await _context.Database.CanConnectAsync();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string GetConnectionState()
        {
            try
            {
                var connection = _context.Database.GetDbConnection();
                return connection.State.ToString();
            }
            catch (Exception ex)
            {
                return $"������ ��������� ���������: {ex.Message}";
            }
        }

        public async Task MigrateAsync()
        {
            try
            {
                await _context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("������ ��� ���������� ��������.", ex);
            }
        }

        public async Task<IEnumerable<string>> GetPendingMigrationsAsync()
        {
            try
            {
                return await _context.Database.GetPendingMigrationsAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("������ ��� ��������� ������ ��������� ��������.", ex);
            }
        }

        public async Task<IEnumerable<string>> GetAppliedMigrationsAsync()
        {
            try
            {
                return await _context.Database.GetAppliedMigrationsAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("������ ��� ��������� ������ ����������� ��������.", ex);
            }
        }

        // ===========================
        // DISPOSABLE PATTERN
        // ===========================
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                try
                {
                    // ���������� �������� ���������� ��� ������������ ��������
                    if (_currentTransaction != null)
                    {
                        _currentTransaction.Rollback();
                        _currentTransaction.Dispose();
                        _currentTransaction = null;
                    }

                    // ������� ��� ������������
                    _repositories.Clear();

                    // ����������� ��������
                    _context?.Dispose();
                }
                catch (Exception)
                {
                    // ���������� ������ ��� ������������ ��������
                }
                finally
                {
                    _disposed = true;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}