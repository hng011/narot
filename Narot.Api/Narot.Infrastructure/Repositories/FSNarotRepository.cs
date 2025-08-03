using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Narot.Application.Interfaces;
using Narot.Domain.Entities;

namespace Narot.Infrastructure.Repositories
{
    [FirestoreData]
    public class FSNarotCard
    {
        [FirestoreProperty("id")] public int Id {get; set;}
        [FirestoreProperty("name")] public string Name { get; set; } = null!;
        [FirestoreProperty("image")] public string Image {get; set; } = null!;
    }

    public class FSNarotRepository : INarotRepository
    {
        private readonly FirestoreDb _firestoreDb;
        private const string CollectionName = "narotDeck";

        public FSNarotRepository(string projectId, string pathCreds)
        {
            var creds = GoogleCredential.FromFile(pathCreds);

            _firestoreDb = new FirestoreDbBuilder
            {
                ProjectId = projectId,
                Credential = creds,
            }.Build();
        }

        public async Task<List<string>> GetDeckNamesAsync()
        {
            var coll = _firestoreDb.Collection(CollectionName);
            var snapshot = await coll.ListDocumentsAsync().ToListAsync();
            return snapshot.Select(d => d.Id).ToList();
        }

        public async Task<NarotDeck> GetDeckAsync(string deckName)
        {
            var doc = _firestoreDb.Collection(CollectionName).Document(deckName);
            var snapshot = await doc.GetSnapshotAsync();

            if (!snapshot.Exists) return null;

            var fsCards = snapshot.GetValue<List<FSNarotCard>>("content");

            var narotDeck = new NarotDeck
            {
                Name = deckName,
                Cards = fsCards.Select(c => new NarotCard
                {
                    Id = c.Id,
                    Name = c.Name,
                    Image = c.Image
                }).ToList(),
            };

            return narotDeck;
        }
    }
}