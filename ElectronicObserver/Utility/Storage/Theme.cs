namespace ElectronicObserver.Utility.Storage;

public class Theme
{
	public static string GetTheme(int mode)
	{
		return mode == 0 ? LightMode : DarkMode;
	}

	const string LightMode = @"
			[
				{
					""name"": ""Light"",
					""basicColors"": {
						""red"": ""#FF0000"",
						""orange"": ""#FFA500"",
						""yellow"": ""#FFFF00"",
						""green"": ""#00FF00"",
						""cyan"": ""#00FFFF"",
						""blue"": ""#0000FF"",
						""magenta"": ""#FF00FF"",
						""violet"": ""#EE82EE""
					},
					""barColors"": [
						[
							""#FF0000"",
							""#FF0000"",
							""#FF8800"",
							""#FF8800"",
							""#FFCC00"",
							""#FFCC00"",
							""#00CC00"",
							""#00CC00"",
							""#0044CC"",
							""#44FF00"",
							""#882222"",
							""#888888""
						],
						[
							""#FF0000"",
							""#FF0000"",
							""#FF4400"",
							""#FF8800"",
							""#FFAA00"",
							""#EEEE00"",
							""#CCEE00"",
							""#00CC00"",
							""#0044CC"",
							""#00FF44"",
							""#882222"",
							""#888888""
						]
					],
					""panelColors"": {
						""foreground"": ""#000000"",
						""background"": ""#F0F0F0"",
						""foreground2"": ""#888888"",
						""background2"": ""#E3E3E3"",
						""statusBarFG"": ""#000000"",
						""statusBarBG"": ""#E3E3E3"",
						""skin"": {
							""panelSplitter"": ""#E3E3E3"",
							""docTabBarFG"": ""#000000"",
							""docTabBarBG"": ""#F0F0F0"",
							""docTabActiveFG"": ""#FFFFFF"",
							""docTabActiveBG"": ""#007ACC"",
							""docTabActiveLostFocusFG"": ""#6D6D6D"",
							""docTabActiveLostFocusBG"": ""#CCCEDB"",
							""docTabInactiveHoverFG"": ""#FFFFFF"",
							""docTabInactiveHoverBG"": ""#1C97EA"",
							""docBtnActiveHoverFG"": ""#FFFFFF"",
							""docBtnActiveHoverBG"": ""#1C97EA"",
							""docBtnActiveLostFocusHoverFG"": ""#717171"",
							""docBtnActiveLostFocusHoverBG"": ""#E6E7ED"",
							""docBtnInactiveHoverFG"": ""#FFFFFF"",
							""docBtnInactiveHoverBG"": ""#52B0EF"",
							""toolTabBarFG"": ""#6D6D6D"",
							""toolTabBarBG"": ""#F0F0F0"",
							""toolTabActive"": ""#007ACC"",
							""toolTitleActiveFG"": ""#FFFFFF"",
							""toolTitleActiveBG"": ""#007ACC"",
							""toolTitleLostFocusFG"": ""#6D6D6D"",
							""toolTitleLostFocusBG"": ""#F0F0F0"",
							""toolTitleDotActive"": ""#50AADC"",
							""toolTitleDotLostFocus"": ""#A0A0A0"",
							""autoHideTabBarFG"": ""#E3E3E3"",
							""autoHideTabBarBG"": ""#F0F0F0"",
							""autoHideTabActive"": ""#007ACC"",
							""autoHideTabInactive"": ""#6D6D6D""
						},
						""fleet"": {
							""repairTimerText"": ""#888888"",
							""conditionText"": ""#000000"",
							""conditionVeryTired"": ""#F08080"",
							""conditionTired"": ""#FFA07A"",
							""conditionLittleTired"": ""#FFE4B5"",
							""conditionSparkle"": ""#90EE90"",
							""equipmentLevel"": ""#006666"",
							""remodelReady"": ""#90EE90""
						},
						""fleetOverview"": {
							""shipDamagedFG"": ""#000000"",
							""shipDamagedBG"": ""#F08080"",
							""expeditionOverFG"": ""#000000"",
							""expeditionOverBG"": ""#90EE90"",
							""tiredRecoveredFG"": ""#000000"",
							""tiredRecoveredBG"": ""#90EE90"",
							""alertNotInExpeditionFG"": ""#000000"",
							""alertNotInExpeditionBG"": ""#90EE90""
						},
						""dock"": {
							""repairFinishedFG"": ""#000000"",
							""repairFinishedBG"": ""#90EE90""
						},
						""arsenal"": {
							""buildCompleteFG"": ""#000000"",
							""buildCompleteBG"": ""#90EE90""
						},
						""hq"": {
							""resOverFG"": ""#000000"",
							""resOverBG"": ""#FFE4B5"",
							""shipOverFG"": ""#000000"",
							""shipOverBG"": ""#F08080"",
							""materialMaxFG"": ""#000000"",
							""materialMaxBG"": ""#F08080"",
							""coinMaxFG"": ""#000000"",
							""coinMaxBG"": ""#F08080"",
							""resLowFG"": ""#000000"",
							""resLowBG"": ""#F08080"",
							""resMaxFG"": ""#000000"",
							""resMaxBG"": ""#F08080""
						},
						""quest"": {
							""typeFG"": ""#000000"",
							""typeHensei"": ""#AAFFAA"",
							""typeShutsugeki"": ""#FFCCCC"",
							""typeEnshu"": ""#DDFFAA"",
							""typeEnsei"": ""#CCFFFF"",
							""typeHokyu"": ""#FFFFCC"",
							""typeKojo"": ""#DDCCBB"",
							""typeKaiso"": ""#DDCCFF"",
							""processLT50"": ""#FF8800"",
							""processLT80"": ""#00CC00"",
							""processLT100"": ""#008800"",
							""processDefault"": ""#0088FF""
						},
						""compass"": {
							""shipClass2"": ""#FF0000"",
							""shipClass3"": ""#FF8800"",
							""shipClass4"": ""#006600"",
							""shipClass5"": ""#880000"",
							""shipClass6"": ""#0088FF"",
							""shipClass7"": ""#0000FF"",
							""shipDestroyed"": ""#FF00FF"",
							""eventKind3"": ""#000080"",
							""eventKind6"": ""#006400"",
							""eventKind5"": ""#8B0000""
						},
						""battle"": {
							""barMVP"": ""#FFE4B5"",
							""textMVP"": ""#000000"",
							""textMVP2"": ""#888888"",
							""barEscaped"": ""#C0C0C0"",
							""textEscaped"": ""#000000"",
							""textEscaped2"": ""#888888"",
							""barBossDamaged"": ""#FFE4E1"",
							""textBossDamaged"": ""#000000"",
							""textBossDamaged2"": ""#888888""
						}
					}
				}
			]
		";

	const string DarkMode = @"
		[
			{
				""name"": ""Dark"",
				""basicColors"": {
					""red"": ""#FF0000"",
					""orange"": ""#FFA500"",
					""yellow"": ""#FFFF00"",
					""green"": ""#00FF00"",
					""cyan"": ""#00FFFF"",
					""blue"": ""#0000FF"",
					""magenta"": ""#FF00FF"",
					""violet"": ""#EE82EE""
				},
				""barColors"": [
					[
						""#FF0000"",
						""#FF0000"",
						""#FF8800"",
						""#FF8800"",
						""#FFCC00"",
						""#FFCC00"",
						""#00CC00"",
						""#00CC00"",
						""#0044CC"",
						""#44FF00"",
						""#882222"",
						""#888888""
					],
					[
						""#FF0000"",
						""#FF0000"",
						""#FF4400"",
						""#FF8800"",
						""#FFAA00"",
						""#EEEE00"",
						""#CCEE00"",
						""#00CC00"",
						""#0044CC"",
						""#00FF44"",
						""#882222"",
						""#888888""
					]
				],
				""panelColors"": {
					""foreground"": ""#FFFFFF"",
					""background"": ""#2D2D30"",
					""foreground2"": ""#AAAAAA"",
					""background2"": ""#3F3F46"",
					""statusBarFG"": ""#FFFFFF"",
					""statusBarBG"": ""#3F3F46"",
					""skin"": {
						""panelSplitter"": ""#3F3F46"",
						""docTabBarFG"": ""#888888"",
						""docTabBarBG"": ""#2D2D30"",
						""docTabActiveFG"": ""#FFFFFF"",
						""docTabActiveBG"": ""#3F3F46"",
						""docTabActiveLostFocusFG"": ""#FFFFFF"",
						""docTabActiveLostFocusBG"": ""#3F3F46"",
						""docTabInactiveHoverFG"": ""#FFFFFF"",
						""docTabInactiveHoverBG"": ""#3F3F46"",
						""docBtnActiveHoverFG"": ""#FFFFFF"",
						""docBtnActiveHoverBG"": ""#3F3F46"",
						""docBtnActiveLostFocusHoverFG"": ""#FFFFFF"",
						""docBtnActiveLostFocusHoverBG"": ""#3F3F46"",
						""docBtnInactiveHoverFG"": ""#FFFFFF"",
						""docBtnInactiveHoverBG"": ""#3F3F46"",
						""toolTabBarFG"": ""#888888"",
						""toolTabBarBG"": ""#2D2D30"",
						""toolTabActive"": ""#FFFFFF"",
						""toolTitleActiveFG"": ""#FFFFFF"",
						""toolTitleActiveBG"": ""#3F3F46"",
						""toolTitleLostFocusFG"": ""#888888"",
						""toolTitleLostFocusBG"": ""#2D2D30"",
						""toolTitleDotActive"": ""#2D2D30"",
						""toolTitleDotLostFocus"": ""#3F3F46"",
						""autoHideTabBarFG"": ""#3F3F46"",
						""autoHideTabBarBG"": ""#2D2D30"",
						""autoHideTabActive"": ""#FFFFFF"",
						""autoHideTabInactive"": ""#888888""
					},
					""fleet"": {
						""repairTimerText"": ""#888888"",
						""conditionText"": ""#2D2D30"",
						""conditionVeryTired"": ""#F08080"",
						""conditionTired"": ""#FFA07A"",
						""conditionLittleTired"": ""#FFE4B5"",
						""conditionSparkle"": ""#90EE90"",
						""equipmentLevel"": ""#00FF00"",
						""remodelReady"": ""#90EE90""
					},
					""fleetOverview"": {
						""shipDamagedFG"": ""#2D2D30"",
						""shipDamagedBG"": ""#F08080"",
						""expeditionOverFG"": ""#2D2D30"",
						""expeditionOverBG"": ""#90EE90"",
						""tiredRecoveredFG"": ""#2D2D30"",
						""tiredRecoveredBG"": ""#90EE90"",
						""alertNotInExpeditionFG"": ""#2D2D30"",
						""alertNotInExpeditionBG"": ""#90EE90""
					},
					""dock"": {
						""repairFinishedFG"": ""#2D2D30"",
						""repairFinishedBG"": ""#90EE90""
					},
					""arsenal"": {
						""buildCompleteFG"": ""#2D2D30"",
						""buildCompleteBG"": ""#90EE90""
					},
					""hq"": {
						""resOverFG"": ""#FFFFFF"",
						""resOverBG"": ""#3F3F46"",
						""shipOverFG"": ""#2D2D30"",
						""shipOverBG"": ""#F08080"",
						""materialMaxFG"": ""#2D2D30"",
						""materialMaxBG"": ""#F08080"",
						""coinMaxFG"": ""#2D2D30"",
						""coinMaxBG"": ""#F08080"",
						""resLowFG"": ""#2D2D30"",
						""resLowBG"": ""#F08080"",
						""resMaxFG"": ""#2D2D30"",
						""resMaxBG"": ""#F08080""
					},
					""quest"": {
						""typeFG"": ""#2D2D30"",
						""typeHensei"": ""#AAFFAA"",
						""typeShutsugeki"": ""#FFCCCC"",
						""typeEnshu"": ""#DDFFAA"",
						""typeEnsei"": ""#CCFFFF"",
						""typeHokyu"": ""#FFFF7F"",
						""typeKojo"": ""#DDCCBB"",
						""typeKaiso"": ""#DDCCFF"",
						""processLT50"": ""#FF8800"",
						""processLT80"": ""#00CC00"",
						""processLT100"": ""#008800"",
						""processDefault"": ""#0088FF""
					},
					""compass"": {
						""shipClass2"": ""#FF0000"",
						""shipClass3"": ""#FF8800"",
						""shipClass4"": ""#00AA00"",
						""shipClass5"": ""#AA0000"",
						""shipClass6"": ""#0088FF"",
						""shipClass7"": ""#0000FF"",
						""shipDestroyed"": ""#FF00FF"",
						""eventKind3"": ""#FFFFFF"",
						""eventKind6"": ""#FFFFFF"",
						""eventKind5"": ""#FFFFFF""
					},
					""battle"": {
						""barMVP"": ""#FFE4B5"",
						""textMVP"": ""#2D2D30"",
						""textMVP2"": ""#3F3F46"",
						""barEscaped"": ""#3F3F46"",
						""textEscaped"": ""#FFFFFF"",
						""textEscaped2"": ""#888888"",
						""barBossDamaged"": ""#FFE4E1"",
						""textBossDamaged"": ""#2D2D30"",
						""textBossDamaged2"": ""#3F3F46""
					}
				}
			}
		]
		";
}
