using Books.Api.Contexts;
using Books.Api.Entities;
using Books.Api.ExternalModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Books.Api.Services
{
    public class BookRepository : IBookRepository, IDisposable
    {
        private readonly BooksContext context;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<BookRepository> logger;
        private CancellationTokenSource cancellationTokenSource;

        public BookRepository(BooksContext context, IHttpClientFactory httpClientFactory, ILogger<BookRepository> logger)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Book> GetBookAsync(Guid id)
        {
            return await this.context.Books.Include(b=> b.Author).FirstOrDefaultAsync(p=>p.Id == id);
        }

        public IEnumerable<Book> GetBooks()
        {
            context.Database.ExecuteSqlRaw("WAITFOR DELAY '00:00:02';");
            return context.Books.Include(b => b.Author).ToList();
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await this.context.Books.Include(b=>b.Author).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksAsync(IEnumerable<Guid> ids)
        {
            return await this.context.Books.Where(b=> ids.Contains(b.Id)).Include(b => b.Author).ToListAsync();
        }

        public void AddBook(Book bookToAdd)
        {
            if (bookToAdd == null)
            {
                throw new ArgumentNullException(nameof(bookToAdd));
            }
            this.context.Books.Add(bookToAdd);
        }

        public async Task<bool> SaveChangesAsync()
        {
            // return true if one or more entities were changed
            return (await this.context.SaveChangesAsync() > 0);
        }

        public async Task<BookCover> GetBookCoverAsync(string coverId)
        {
            var httpClient = this.httpClientFactory.CreateClient();
            // pass through a dummy name
            var response = await httpClient.GetAsync($"https://localhost:44398/api/bookcovers/{coverId}");
            if (response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<BookCover>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });
            }

            return null;
        }

        private async Task<BookCover> DownloadBookCoverAsync(HttpClient httpClient, string bookCoverUrl, CancellationToken cancellationToken)
        {
            // throw new Exception("Cannot download book cover, writer isn't finishing book fast enough.");

            var response = await httpClient.GetAsync(bookCoverUrl, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var bookCover = JsonSerializer.Deserialize<BookCover>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    });
                return bookCover;
            }

            cancellationTokenSource.Cancel();
            return null;
        }


        public async Task<IEnumerable<BookCover>> GetBookCoversAsync(Guid bookId)
        {
            var httpClient = this.httpClientFactory.CreateClient();
            var bookCovers = new List<BookCover>();
            cancellationTokenSource = new CancellationTokenSource();

            // create a list of fake bookcovers
            var bookCoverUrls = new[]
            {
                $"https://localhost:44398/api/bookcovers/{bookId}-dummycover1",
                // $"https://localhost:44398/api/bookcovers/{bookId}-dummycover2?returnFault=true",
                $"https://localhost:44398/api/bookcovers/{bookId}-dummycover2",
                $"https://localhost:44398/api/bookcovers/{bookId}-dummycover3",
                $"https://localhost:44398/api/bookcovers/{bookId}-dummycover4",
                $"https://localhost:44398/api/bookcovers/{bookId}-dummycover5"
            };

            // create the tasks
            var downloadBookCoverTasksQuery = from bookCoverUrl in bookCoverUrls
                 select DownloadBookCoverAsync(httpClient, bookCoverUrl, cancellationTokenSource.Token);

            // start the tasks
            var downloadBookCoverTasks = downloadBookCoverTasksQuery.ToList();
            try
            {
                return await Task.WhenAll(downloadBookCoverTasks);
            }
            catch (OperationCanceledException operationCanceledException)
            {
                this.logger.LogInformation($"{operationCanceledException.Message}");
                foreach (var task in downloadBookCoverTasks)
                {
                    this.logger.LogInformation($"Task {task.Id} has status {task.Status}");
                }

                return new List<BookCover>();
            }
            catch (Exception exception)
            {
                this.logger.LogError($"{exception.Message}");
                throw;
            }

            //foreach (var bookCoverUrl in bookCoverUrls)
            //{
            //    var response = await httpClient
            //       .GetAsync(bookCoverUrl);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        bookCovers.Add(JsonSerializer.Deserialize<BookCover>(
            //            await response.Content.ReadAsStringAsync(),
            //            new JsonSerializerOptions
            //            {
            //                PropertyNameCaseInsensitive = true,
            //            }));
            //    }
            //}

            //return bookCovers;
        }


        #region Dispose
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if(context != null)
                    {
                        context.Dispose();
                    }
                    if (cancellationTokenSource != null)
                    {
                        cancellationTokenSource.Dispose();
                        cancellationTokenSource = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~BookRepository()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
