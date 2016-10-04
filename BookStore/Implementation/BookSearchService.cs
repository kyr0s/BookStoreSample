using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BookStore.Implementation.DataProviders;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Version = Lucene.Net.Util.Version;

namespace BookStore.Implementation
{
    public class BookSearchService : IBookSearchService
    {
        private const Version version = Version.LUCENE_30;
        private const string isbnFieldName = "isbn";
        private const string authorFieldName = "author";
        private const string titleFieldName = "title";

        private IndexSearcher searcher;
        private Dictionary<string, BookWrapper> booksByIsbn;
        private readonly Regex queryCleaner;

        private readonly IBookDataProvider bookDataProvider;

        public BookSearchService(IBookDataProvider bookDataProvider)
        {
            this.bookDataProvider = bookDataProvider;
            queryCleaner = new Regex(@"[^\w\d\s-]*", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }

        public BookWrapper[] Search(string query, int count)
        {
            var processedQuery = queryCleaner.Replace(query, string.Empty);
            if (string.IsNullOrWhiteSpace(processedQuery))
            {
                return booksByIsbn.Values
                    .Take(count)
                    .ToArray();
            }

            var isbnQuery = new PrefixQuery(new Term(isbnFieldName, processedQuery));
            var authorQuery = CreatePhraseQuery(authorFieldName, processedQuery);
            var titleQuery = CreatePhraseQuery(titleFieldName, processedQuery);

            var booleanQuery = new BooleanQuery();
            booleanQuery.Clauses.Add(new BooleanClause(isbnQuery, Occur.SHOULD));
            booleanQuery.Clauses.Add(new BooleanClause(authorQuery, Occur.SHOULD));
            booleanQuery.Clauses.Add(new BooleanClause(titleQuery, Occur.SHOULD));

            var searchResult = searcher.Search(booleanQuery, count);
            var foundIsbns = searchResult.ScoreDocs
                .Select(d => searcher.Doc(d.Doc))
                .Select(d => d.Get(isbnFieldName))
                .ToArray();

            var foundBooks = foundIsbns.Select(isbn => booksByIsbn[isbn]).ToArray();
            return foundBooks;
        }

        public void InitializeIndex()
        {
            var books = bookDataProvider.SelectAll();
            booksByIsbn = books.ToDictionary(b => b.Book.ISBN);
            var directory = new RAMDirectory();

            using (var indexWriter = new IndexWriter(directory, new StandardAnalyzer(version), true, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (var book in books)
                {
                    AddToIndex(book, indexWriter);
                }
                indexWriter.Optimize();
            }

            searcher = new IndexSearcher(directory, true);
        }

        private static Query CreatePhraseQuery(string field, string query)
        {
            var phraseQuery = new PhraseQuery {Slop = 3};
            phraseQuery.Add(new Term(field, query));
            return phraseQuery;
        }

        private static void AddToIndex(BookWrapper bookWrapper, IndexWriter writer)
        {
            var doc = new Document();

            doc.Add(new Field(isbnFieldName, bookWrapper.Book.ISBN, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(authorFieldName, bookWrapper.Book.Author, Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field(titleFieldName, bookWrapper.Book.Title, Field.Store.NO, Field.Index.ANALYZED));

            writer.AddDocument(doc);
        }
    }
}