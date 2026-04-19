// DAL/Interfaces/IUnitOfWork.cs

using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL.Interfaces
{
    /// <summary>
    /// Unit of Work      
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IAnecdoteRepository Anecdotes { get; }
        INameMainRepository NameMains { get; }
        IIconCategoryRepository IconCategories { get; }
        IIconRepository Icons { get; }
        ILanguageAppRepository LanguagesApp { get; }
        ILanguageOfAggregatorRepository LanguagesOfAggregator { get; }
        IPlatformOfAggregatorRepository PlatformsOfAggregator { get; }
        IPlatformRepository Platforms { get; }

        /// <summary>
        ///     Samples
        /// </summary>
        ISampleRepository Samples { get; }

        ISampleMainRepository SamplesMain { get; }
        ISampleMainDescriptionRepository SamplesMainDescriptions { get; }
        ISampleMainSeoRepository SamplesMainSeo { get; }

        /// <summary>
        /// ����������� ��� ������ � �������
        /// </summary>
        ILanguageRepository Languages { get; }

        /// <summary>
        /// ����������� ��� ������ � �������
        /// </summary>
        INameRepository Names { get; }

        /// <summary>
        /// ����������� ��� ������ � ���������� ����
        /// </summary>
        INameDescriptionRepository NameDescriptions { get; }

        /// <summary>
        /// ����������� ��� ������ � SEO
        /// </summary>
        ISeoDataRepository SeoDatas { get; }

        // ===========================
        // �������� ���������
        // ===========================
        /// <summary>
        /// ������ � DbContext
        /// </summary>
        DbContext Context { get; }

        // ===========================
        // ������������������ �����������
        // ===========================
        /// <summary>
        /// ����������� ��� ������ � ��������������
        /// </summary>
        IUserRepository Users { get; }

        /// <summary>
        /// ����������� ��� ������ � �������� �������������
        /// </summary>
        IUserSessionRepository UserSessions { get; }

        /// <summary>
        /// ����������� ��� ������ � ������ ����������
        /// </summary>
        IActivityLogRepository ActivityLogs { get; }
        IUserSettingsRepository UserSettings { get; }

        // ===========================
        // GENERIC REPOSITORY
        // ===========================
        /// <summary>
        /// �������� generic ����������� ��� ����� ��������
        /// </summary>
        /// <typeparam name="TEntity">��� ��������</typeparam>
        /// <returns>����������� ��� ������ � ���������</returns>
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        // ===========================
        // ���������� ���������
        // ===========================
        /// <summary>
        /// ���������� ���������� ���������
        /// </summary>
        /// <returns>���������� ���������� �������</returns>
        int SaveChanges();

        /// <summary>
        /// ����������� ���������� ���������
        /// </summary>
        /// <param name="cancellationToken">����� ������</param>
        /// <returns>���������� ���������� �������</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        // ===========================
        // ���������� - ���������� ������
        // ===========================
        /// <summary>
        /// �������� ����� ���������� (���������� �����)
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// ������������ ������� ���������� (���������� �����)
        /// </summary>
        Task CommitTransactionAsync();

        /// <summary>
        /// ���������� ������� ���������� (���������� �����)
        /// </summary>
        Task RollbackTransactionAsync();

        // ===========================
        // ���������� - ����������� ������
        // ===========================
        /// <summary>
        /// �������� ���������� � ���������� ������ ����������
        /// </summary>
        /// <param name="cancellationToken">����� ������</param>
        /// <returns>������ ����������</returns>
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// ��������� �������� � ���������� � �������������� commit/rollback
        /// </summary>
        /// <param name="action">�������� ��� ����������</param>
        /// <param name="cancellationToken">����� ������</param>
        Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// ��������� �������� � ���������� � ������������ ����������
        /// </summary>
        /// <typeparam name="T">��� ������������� ����������</typeparam>
        /// <param name="action">�������� ��� ����������</param>
        /// <param name="cancellationToken">����� ������</param>
        /// <returns>��������� ���������� ��������</returns>
        Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default);

        // ===========================
        // SQL �������
        // ===========================
        /// <summary>
        /// ��������� ����� SQL ������
        /// </summary>
        /// <param name="sql">SQL ������</param>
        /// <param name="cancellationToken">����� ������</param>
        /// <returns>���������� ���������� �������</returns>
        Task<int> ExecuteSqlRawAsync(string sql, CancellationToken cancellationToken = default);

        /// <summary>
        /// ��������� SQL ������ � �����������
        /// </summary>
        /// <param name="sql">SQL ������</param>
        /// <param name="parameters">��������� �������</param>
        /// <returns>���������� ���������� �������</returns>
        Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters);

        /// <summary>
        /// Выполняет полную очистку таблицы (PostgreSQL TRUNCATE).
        /// В отличие от удаления через DELETE, этот метод работает быстрее и позволяет сбросить счетчики ID.
        /// </summary>
        /// <param name="tableName">Имя таблицы в БД</param>
        /// <param name="cascade">Если true, будут также удалены связанные данные в других таблицах (CASCADE)</param>
        /// <returns>Количество затронутых строк</returns>
        Task<int> TruncateTableAsync(string tableName, bool cascade = true);

        // ===========================
        // ���������� ����������
        // ===========================
        /// <summary>
        /// ����������� ��� �������� �� ���������
        /// </summary>
        void DetachAllEntities();

        /// <summary>
        /// ���������� ������������� ��� ������� (PostgreSQL ������)
        /// </summary>
        /// <typeparam name="TEntity">��� ��������</typeparam>
        Task ResetSequenceAsync<TEntity>() where TEntity : class;

        /// <summary>
        /// ���������, ���� �� ������������� ���������
        /// </summary>
        /// <returns>True, ���� ���� ������������� ���������</returns>
        bool HasUnsavedChanges();

        /// <summary>
        /// �������� ��� ��������� � ���������
        /// </summary>
        void RejectChanges();

        // ===========================
        // �������������� ������
        // ===========================
        /// <summary>
        /// ��������� ����������� ���� ������
        /// </summary>
        /// <returns>True, ���� ���� ������ ��������</returns>
        Task<bool> CanConnectAsync();

        /// <summary>
        /// �������� ���������� � ��������� �����������
        /// </summary>
        /// <returns>��������� �����������</returns>
        string GetConnectionState();

        /// <summary>
        /// ��������� �������� ���� ������
        /// </summary>
        Task MigrateAsync();

        /// <summary>
        /// �������� ������ ��������� ��������
        /// </summary>
        /// <returns>������ ��������</returns>
        Task<IEnumerable<string>> GetPendingMigrationsAsync();

        /// <summary>
        /// �������� ������ ����������� ��������
        /// </summary>
        /// <returns>������ ��������</returns>
        Task<IEnumerable<string>> GetAppliedMigrationsAsync();
    }
}