mergeInto(LibraryManager.library, {

  IsItMobile: function(){
    setIsMobile();
  },

  GetLang: function(){
    getLang();
  },

  PlayerAuth: function(){
    auth();
  },

  PlayerStartAuth: function(){
    startAuth();
  },

  GetUserData: function(){
    getUserData();
  },

  SetUserLiderScore: function(_score){
    setUserLiderScore(_score);
  },
      
  SetUserData: function(_data){
    var dateString = UTF8ToString(_data);
    var myObj = JSON.parse(dateString);
    setUserData(myObj);
  },

  ShowCommonAdv: function(){
    showFullscreenADV();
  },

  ShowRewardAdv: function(_rewardType){
    showRewardedADV(_rewardType);
  },

  RateGame: function(){
    ysdk.feedback.canReview()
        .then(({ value, reason }) => {
            if (value) {
                myGameInstance.SendMessage('ManagerGame', 'PauseForAdv', "true");
                ysdk.feedback.requestReview()
                    .then(({ feedbackSent }) => {
                        console.log(feedbackSent);
                        myGameInstance.SendMessage('ManagerGame', 'PauseForAdv', "flase");
                    })
            } else {
                console.log(reason)
                myGameInstance.SendMessage('ManagerGame', 'PauseForAdv', "false");
            }
        })
  },

});