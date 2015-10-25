using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuzzyLogicModel;

namespace FuzzyLogicWebService.Models
{
    public interface IDatabaseRepository
    {
        User GetUserByLogin(string login);
        void RegisterUser(User user);
        FuzzyModel GetModelById(int modelId);
        FuzzyModel EditModel(FuzzyModel newModel);
        List<FuzzyModel> GetUserModels(int userID);
        FuzzyModel AddModelForUser(int userId, FuzzyModel model);
        void AddInputVariableForModel(int modelId, IEnumerable<FuzzyVariable> variables);
        FuzzyVariable GetVariableById(int variableId);
        void AddOutputVariableForModel(int modelId, IEnumerable<FuzzyVariable> variables);
        void AddRulesToModel(int modelId, IEnumerable<FuzzyRule> rules);
        List<MembershipFunction> AddMembFuncForVariable(int variableId, List<MembershipFunction> listOfMfs);
        IQueryable<FuzzyVariable> GetVariablesForModel(int modelId);
        IQueryable<MembershipFunction> GetMfForVariable(int variableId);
        IQueryable<FuzzyRule> GetRulesForModel(int modelId);
        void UpdateModelStatus(int modelID, int status);
        void UpdateModelStatus(FuzzyModel model, int status);
        void CopyGivenModel(int modelId, int userId);
        void SaveEditedModel(FuzzyModel updatedModel);
        void DeleteModelById(int id);
    }
}
