using BobTheBot.Entities;
using BobTheBot.Kernel;
using BobTheBot.RequestAndResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BobTheBot.ApplicationServices
{
    public class SearchKeyService
    {
        private readonly IUnitOfWork unitOfWork;

        public SearchKeyService(
           IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<HttpResponseMessage> Add(WordsRequest request)
        {
            var existingWordsEntity = await unitOfWork.SearchKeyRepository.GetAllWords();
            var existingWords = existingWordsEntity.Select(x => x.Word);

            foreach (var item in request.Words)
            {
                if (!existingWords.Contains(item))
                {
                    var word = new SearchKey(item);
                    await unitOfWork.SearchKeyRepository.InsertAsync(word);
                }
            }
            await unitOfWork.SaveChangesAsync();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        public async Task<IEnumerable<WordsResponse>> GetAsync()
        {
            var existingWords = await unitOfWork.SearchKeyRepository.GetAllWords();
            return existingWords.Select(x => WordsResponse.Create(x));
        }

        public async Task<HttpResponseMessage> DeleteAsync(int wordId)
        {
            var word = await unitOfWork.SearchKeyRepository.GetWordById(wordId);
            if (word == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            unitOfWork.SearchKeyRepository.Delete(word);
            await unitOfWork.SaveChangesAsync();
            return new HttpResponseMessage(HttpStatusCode.OK);

        }
    }
}
