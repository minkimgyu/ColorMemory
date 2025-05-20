using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Collect
{
    public class ShareState : BaseState<CollectMode.State>
    {
        Animator _shareAnimator;
        CollectStageUIPresenter _collectStageUIPresenter;

        ShareComponent _shareComponent;

        Dictionary<int, Sprite> _artSpriteAsserts;
        ArtworkDateWrapper _artworkJsonDataAssets;

        public ShareState(
            FSM<CollectMode.State> fsm,
            Animator shareAnimator,

            ShareComponent shareComponent,

            Dictionary<int, Sprite> artSpriteAsserts,
            ArtworkDateWrapper artworkJsonDataAssets,

            CollectStageUIPresenter collectStageUIPresenter) : base(fsm)
        {
            _shareAnimator = shareAnimator;

            _artSpriteAsserts = artSpriteAsserts;
            _artworkJsonDataAssets = artworkJsonDataAssets;

            _collectStageUIPresenter = collectStageUIPresenter;

            _shareComponent = shareComponent;
            _collectStageUIPresenter.OnShareButtonClick += _shareComponent.OnShareButtonClick;
            _collectStageUIPresenter.ExitShareState += () => 
            {
                _collectStageUIPresenter.ActivateGameResultPanel(true);
                _collectStageUIPresenter.ActivateSharePanel(false); 
            };

            _shareComponent.Initialize(
                () => _collectStageUIPresenter.ActivateShareBottomItems(false),
                () => _collectStageUIPresenter.ActivateShareBottomItems(true));
        }

        const int _artworkCount = 5;

        List<int> PickRandomIndexes(List<int> randomIndexes)
        {
            while (randomIndexes.Count < _artworkCount)
            {
                int randomIndex = Random.Range(0, _artSpriteAsserts.Count);
                if(randomIndexes.Contains(randomIndex) == false) randomIndexes.Add(randomIndex);
            }

            return randomIndexes;
        }

        public override void OnStateEnter()
        {
            _collectStageUIPresenter.ActivateGameResultPanel(false);
            _collectStageUIPresenter.ActivateSharePanel(true);

            string shareTitle = ServiceLocater.ReturnLocalizationManager().GetWord(ILocalization.Key.ShareArtwork);
            _collectStageUIPresenter.ChangeShareTitle(shareTitle);

            List<int> randomIndexes = new List<int>();

            SaveData data = ServiceLocater.ReturnSaveManager().GetSaveData();
            randomIndexes.Add(data.SelectedArtworkKey);

            randomIndexes = PickRandomIndexes(randomIndexes);

            Sprite[] shareArtSprites = new Sprite[randomIndexes.Count];
            ArtworkData[] shareArtworkDatas = new ArtworkData[randomIndexes.Count];

            for (int i = 0; i < randomIndexes.Count; i++)
            {
                if (_artSpriteAsserts.ContainsKey(randomIndexes[i]) == false ||
                    _artworkJsonDataAssets.Data.ContainsKey(randomIndexes[i]) == false) continue;

                shareArtSprites[i] = _artSpriteAsserts[randomIndexes[i]];
                shareArtworkDatas[i] = _artworkJsonDataAssets.Data[randomIndexes[i]];
            }

            _collectStageUIPresenter.ChangeShareArtworks(shareArtSprites, shareArtworkDatas);
            _shareAnimator.SetTrigger("Share");
        }
    }
}