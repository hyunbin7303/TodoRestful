﻿using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TodoApi.Datasource
{
    //source :https://gist.github.com/marekzyla/392a551ce0f45d5a653ea6872e7e0f86
    public class TodoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : IDocument
    {
        private readonly IMongoCollection<TDocument> _collection;
        public TodoRepository(IMongoSettings settings)
        {
            var client = new MongoClient(settings.Connection);
            var db = client.GetDatabase(settings.DatabaseName);
            _collection = db.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }
        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
        }
        public virtual IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }
        public virtual void DeleteById(string id)
        {
            var objId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(d => d.Id, objId);
            _collection.FindOneAndDelete(filter);
        }
        public virtual Task DeleteByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
                _collection.FindOneAndDeleteAsync(filter);
            });
        }
        public virtual void DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.DeleteMany(filterExpression);
        }
        public virtual Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.FindOneAndDeleteAsync(filterExpression));
        }
        public virtual void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            _collection.FindOneAndDelete(filterExpression);
        }
        public virtual Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.FindOneAndDeleteAsync(filterExpression));
        }
        public virtual IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }
        public virtual IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }
        public TDocument FindById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TDocument>.Filter.Eq(d => d.Id, objectId);
            return _collection.Find(filter).SingleOrDefault();
        }
        public Task<TDocument> FindByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
                return _collection.Find(filter).SingleOrDefaultAsync();
            });
        }
        public TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
        {
            return Task.Run(() => _collection.Find(filterExpression).FirstOrDefaultAsync());
        }

        public void InsertMany(ICollection<TDocument> documents)
        {
            throw new NotImplementedException();
        }
        public virtual async Task InsertManyAsync(ICollection<TDocument> documents)
        {
            await _collection.InsertManyAsync(documents);
        }
        public virtual void InsertOne(TDocument document)
        {
            _collection.InsertOne(document);
        }
        public virtual Task InsertOneAsync(TDocument document)
        {
            return Task.Run(() => _collection.InsertOneAsync(document));
        }
        public void ReplaceOne(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            _collection.FindOneAndReplace(filter, document);
        }
        public virtual async Task ReplaceOneAsync(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }
        public Task<IList<TDocument>> FindAll()
        {
            return Task.FromResult<IList<TDocument>>(_collection.Find(Builders<TDocument>.Filter.Empty).ToList());
        }
        public Task<IList<TDocument>> Search(string keyword)
        {
            var filter = Builders<TDocument>.Filter.Text(keyword);
            return Task.FromResult<IList<TDocument>>(_collection.Find(filter).ToList());
        }

        public IEnumerable<TDocument> AllIncluding(params Expression<Func<TDocument, object>>[] includeProperties)
        {
            //IQueryable<TDocument> query = _context.Set<TDocument>();
            //foreach (var includeProperty in includeProperties)
            //{
            //    query = query.Include(includeProperty);
            //}
            //return query.AsEnumerable();
            return null;
        }
    }
}
