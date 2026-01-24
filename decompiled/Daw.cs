using System.Collections;
using UnityEngine;

public class Daw : Wrapper
{
	[Header("Children")]
	public TimelineTabs TimelineTabs;

	public Highlighter Highlighter;

	public BarSlot[] BarSlots;

	public Wrapper Wrapper;

	[Header("Fragments")]
	public Fragment activator;

	private float wrapperInitLocalX;

	private float wrapperActiveLocalX;

	private float wrapperNextLocalX;

	private float wrapperInitLocalY;

	private float wrapperLocalZ;

	private float speed = 7.8f;

	private float timeTilActivated;

	private static int activeBeatSlotNum;

	private static int activeBarSlotNum;

	private Coroutine changingBeat;

	private Coroutine increasingBeatLinear;

	protected override void Awake()
	{
		activator.Awake();
		timeTilActivated = activator.GetAnimDuration("activate");
		wrapperInitLocalX = Wrapper.GetLocalX();
		wrapperActiveLocalX = Wrapper.GetLocalX();
		wrapperNextLocalX = Wrapper.GetLocalX();
		wrapperInitLocalY = Wrapper.GetLocalY();
		wrapperLocalZ = Wrapper.GetLocalZ();
	}

	public void Activate(bool isReactivated, char initiatedDataType)
	{
		editorData editorData2 = SaveManager.mgr.GetEditorData();
		if (!isReactivated)
		{
			activator.TriggerAnim("activate");
		}
		TimelineTabs.Show();
		Highlighter.Show();
		RefreshBars();
		for (int i = 0; i < 4; i++)
		{
			BarSlots[0].InitiateCode(i, editorData2.customCodedBar1[i], initiatedDataType);
		}
		for (int j = 0; j < 4; j++)
		{
			BarSlots[1].InitiateCode(j, editorData2.customCodedBar2[j], initiatedDataType);
		}
		for (int k = 0; k < 4; k++)
		{
			BarSlots[2].InitiateCode(k, editorData2.customCodedBar3[k], initiatedDataType);
		}
		for (int l = 0; l < 4; l++)
		{
			BarSlots[3].InitiateCode(l, editorData2.customCodedBar4[l], initiatedDataType);
		}
		for (int m = 0; m < 4; m++)
		{
			BarSlots[4].InitiateCode(m, editorData2.customCodedBar5[m], initiatedDataType);
		}
		for (int n = 0; n < 4; n++)
		{
			BarSlots[5].InitiateCode(n, editorData2.customCodedBar6[n], initiatedDataType);
		}
		for (int num = 0; num < 4; num++)
		{
			BarSlots[6].InitiateCode(num, editorData2.customCodedBar7[num], initiatedDataType);
		}
		for (int num2 = 0; num2 < 4; num2++)
		{
			BarSlots[7].InitiateCode(num2, editorData2.customCodedBar8[num2], initiatedDataType);
		}
		for (int num3 = 0; num3 < 4; num3++)
		{
			BarSlots[8].InitiateCode(num3, editorData2.customCodedBar9[num3], initiatedDataType);
		}
		for (int num4 = 0; num4 < 4; num4++)
		{
			BarSlots[9].InitiateCode(num4, editorData2.customCodedBar10[num4], initiatedDataType);
		}
		for (int num5 = 0; num5 < 4; num5++)
		{
			BarSlots[10].InitiateCode(num5, editorData2.customCodedBar11[num5], initiatedDataType);
		}
		for (int num6 = 0; num6 < 4; num6++)
		{
			BarSlots[11].InitiateCode(num6, editorData2.customCodedBar12[num6], initiatedDataType);
		}
		for (int num7 = 0; num7 < 4; num7++)
		{
			BarSlots[12].InitiateCode(num7, editorData2.customCodedBar13[num7], initiatedDataType);
		}
		for (int num8 = 0; num8 < 4; num8++)
		{
			BarSlots[13].InitiateCode(num8, editorData2.customCodedBar14[num8], initiatedDataType);
		}
		for (int num9 = 0; num9 < 4; num9++)
		{
			BarSlots[14].InitiateCode(num9, editorData2.customCodedBar15[num9], initiatedDataType);
		}
		for (int num10 = 0; num10 < 4; num10++)
		{
			BarSlots[15].InitiateCode(num10, editorData2.customCodedBar16[num10], initiatedDataType);
		}
		for (int num11 = 0; num11 < 4; num11++)
		{
			BarSlots[16].InitiateCode(num11, editorData2.customCodedBar17[num11], initiatedDataType);
		}
		for (int num12 = 0; num12 < 4; num12++)
		{
			BarSlots[17].InitiateCode(num12, editorData2.customCodedBar18[num12], initiatedDataType);
		}
		for (int num13 = 0; num13 < 4; num13++)
		{
			BarSlots[18].InitiateCode(num13, editorData2.customCodedBar19[num13], initiatedDataType);
		}
		for (int num14 = 0; num14 < 4; num14++)
		{
			BarSlots[19].InitiateCode(num14, editorData2.customCodedBar20[num14], initiatedDataType);
		}
		for (int num15 = 0; num15 < 4; num15++)
		{
			BarSlots[20].InitiateCode(num15, editorData2.customCodedBar21[num15], initiatedDataType);
		}
		for (int num16 = 0; num16 < 4; num16++)
		{
			BarSlots[21].InitiateCode(num16, editorData2.customCodedBar22[num16], initiatedDataType);
		}
		for (int num17 = 0; num17 < 4; num17++)
		{
			BarSlots[22].InitiateCode(num17, editorData2.customCodedBar23[num17], initiatedDataType);
		}
		for (int num18 = 0; num18 < 4; num18++)
		{
			BarSlots[23].InitiateCode(num18, editorData2.customCodedBar24[num18], initiatedDataType);
		}
		for (int num19 = 0; num19 < 4; num19++)
		{
			BarSlots[24].InitiateCode(num19, editorData2.customCodedBar25[num19], initiatedDataType);
		}
		for (int num20 = 0; num20 < 4; num20++)
		{
			BarSlots[25].InitiateCode(num20, editorData2.customCodedBar26[num20], initiatedDataType);
		}
		for (int num21 = 0; num21 < 4; num21++)
		{
			BarSlots[26].InitiateCode(num21, editorData2.customCodedBar27[num21], initiatedDataType);
		}
		for (int num22 = 0; num22 < 4; num22++)
		{
			BarSlots[27].InitiateCode(num22, editorData2.customCodedBar28[num22], initiatedDataType);
		}
		for (int num23 = 0; num23 < 4; num23++)
		{
			BarSlots[28].InitiateCode(num23, editorData2.customCodedBar29[num23], initiatedDataType);
		}
		for (int num24 = 0; num24 < 4; num24++)
		{
			BarSlots[29].InitiateCode(num24, editorData2.customCodedBar30[num24], initiatedDataType);
		}
		for (int num25 = 0; num25 < 4; num25++)
		{
			BarSlots[30].InitiateCode(num25, editorData2.customCodedBar31[num25], initiatedDataType);
		}
		for (int num26 = 0; num26 < 4; num26++)
		{
			BarSlots[31].InitiateCode(num26, editorData2.customCodedBar32[num26], initiatedDataType);
		}
		for (int num27 = 0; num27 < 4; num27++)
		{
			BarSlots[32].InitiateCode(num27, editorData2.customCodedBar33[num27], initiatedDataType);
		}
		for (int num28 = 0; num28 < 4; num28++)
		{
			BarSlots[33].InitiateCode(num28, editorData2.customCodedBar34[num28], initiatedDataType);
		}
		for (int num29 = 0; num29 < 4; num29++)
		{
			BarSlots[34].InitiateCode(num29, editorData2.customCodedBar35[num29], initiatedDataType);
		}
		for (int num30 = 0; num30 < 4; num30++)
		{
			BarSlots[35].InitiateCode(num30, editorData2.customCodedBar36[num30], initiatedDataType);
		}
		for (int num31 = 0; num31 < 4; num31++)
		{
			BarSlots[36].InitiateCode(num31, editorData2.customCodedBar37[num31], initiatedDataType);
		}
		for (int num32 = 0; num32 < 4; num32++)
		{
			BarSlots[37].InitiateCode(num32, editorData2.customCodedBar38[num32], initiatedDataType);
		}
		for (int num33 = 0; num33 < 4; num33++)
		{
			BarSlots[38].InitiateCode(num33, editorData2.customCodedBar39[num33], initiatedDataType);
		}
		for (int num34 = 0; num34 < 4; num34++)
		{
			BarSlots[39].InitiateCode(num34, editorData2.customCodedBar40[num34], initiatedDataType);
		}
		for (int num35 = 0; num35 < 4; num35++)
		{
			BarSlots[40].InitiateCode(num35, editorData2.customCodedBar41[num35], initiatedDataType);
		}
		for (int num36 = 0; num36 < 4; num36++)
		{
			BarSlots[41].InitiateCode(num36, editorData2.customCodedBar42[num36], initiatedDataType);
		}
		for (int num37 = 0; num37 < 4; num37++)
		{
			BarSlots[42].InitiateCode(num37, editorData2.customCodedBar43[num37], initiatedDataType);
		}
		for (int num38 = 0; num38 < 4; num38++)
		{
			BarSlots[43].InitiateCode(num38, editorData2.customCodedBar44[num38], initiatedDataType);
		}
		for (int num39 = 0; num39 < 4; num39++)
		{
			BarSlots[44].InitiateCode(num39, editorData2.customCodedBar45[num39], initiatedDataType);
		}
		for (int num40 = 0; num40 < 4; num40++)
		{
			BarSlots[45].InitiateCode(num40, editorData2.customCodedBar46[num40], initiatedDataType);
		}
		for (int num41 = 0; num41 < 4; num41++)
		{
			BarSlots[46].InitiateCode(num41, editorData2.customCodedBar47[num41], initiatedDataType);
		}
		for (int num42 = 0; num42 < 4; num42++)
		{
			BarSlots[47].InitiateCode(num42, editorData2.customCodedBar48[num42], initiatedDataType);
		}
		for (int num43 = 0; num43 < 4; num43++)
		{
			BarSlots[48].InitiateCode(num43, editorData2.customCodedBar49[num43], initiatedDataType);
		}
		for (int num44 = 0; num44 < 4; num44++)
		{
			BarSlots[49].InitiateCode(num44, editorData2.customCodedBar50[num44], initiatedDataType);
		}
		for (int num45 = 0; num45 < 4; num45++)
		{
			BarSlots[50].InitiateCode(num45, editorData2.customCodedBar51[num45], initiatedDataType);
		}
		for (int num46 = 0; num46 < 4; num46++)
		{
			BarSlots[51].InitiateCode(num46, editorData2.customCodedBar52[num46], initiatedDataType);
		}
		for (int num47 = 0; num47 < 4; num47++)
		{
			BarSlots[52].InitiateCode(num47, editorData2.customCodedBar53[num47], initiatedDataType);
		}
		for (int num48 = 0; num48 < 4; num48++)
		{
			BarSlots[53].InitiateCode(num48, editorData2.customCodedBar54[num48], initiatedDataType);
		}
		for (int num49 = 0; num49 < 4; num49++)
		{
			BarSlots[54].InitiateCode(num49, editorData2.customCodedBar55[num49], initiatedDataType);
		}
		for (int num50 = 0; num50 < 4; num50++)
		{
			BarSlots[55].InitiateCode(num50, editorData2.customCodedBar56[num50], initiatedDataType);
		}
		for (int num51 = 0; num51 < 4; num51++)
		{
			BarSlots[56].InitiateCode(num51, editorData2.customCodedBar57[num51], initiatedDataType);
		}
		for (int num52 = 0; num52 < 4; num52++)
		{
			BarSlots[57].InitiateCode(num52, editorData2.customCodedBar58[num52], initiatedDataType);
		}
		for (int num53 = 0; num53 < 4; num53++)
		{
			BarSlots[58].InitiateCode(num53, editorData2.customCodedBar59[num53], initiatedDataType);
		}
		for (int num54 = 0; num54 < 4; num54++)
		{
			BarSlots[59].InitiateCode(num54, editorData2.customCodedBar60[num54], initiatedDataType);
		}
		for (int num55 = 0; num55 < 4; num55++)
		{
			BarSlots[60].InitiateCode(num55, editorData2.customCodedBar61[num55], initiatedDataType);
		}
		for (int num56 = 0; num56 < 4; num56++)
		{
			BarSlots[61].InitiateCode(num56, editorData2.customCodedBar62[num56], initiatedDataType);
		}
		for (int num57 = 0; num57 < 4; num57++)
		{
			BarSlots[62].InitiateCode(num57, editorData2.customCodedBar63[num57], initiatedDataType);
		}
		for (int num58 = 0; num58 < 4; num58++)
		{
			BarSlots[63].InitiateCode(num58, editorData2.customCodedBar64[num58], initiatedDataType);
		}
		for (int num59 = 0; num59 < 4; num59++)
		{
			BarSlots[64].InitiateCode(num59, editorData2.customCodedBar65[num59], initiatedDataType);
		}
		for (int num60 = 0; num60 < 4; num60++)
		{
			BarSlots[65].InitiateCode(num60, editorData2.customCodedBar66[num60], initiatedDataType);
		}
		for (int num61 = 0; num61 < 4; num61++)
		{
			BarSlots[66].InitiateCode(num61, editorData2.customCodedBar67[num61], initiatedDataType);
		}
		for (int num62 = 0; num62 < 4; num62++)
		{
			BarSlots[67].InitiateCode(num62, editorData2.customCodedBar68[num62], initiatedDataType);
		}
		for (int num63 = 0; num63 < 4; num63++)
		{
			BarSlots[68].InitiateCode(num63, editorData2.customCodedBar69[num63], initiatedDataType);
		}
		for (int num64 = 0; num64 < 4; num64++)
		{
			BarSlots[69].InitiateCode(num64, editorData2.customCodedBar70[num64], initiatedDataType);
		}
		for (int num65 = 0; num65 < 4; num65++)
		{
			BarSlots[70].InitiateCode(num65, editorData2.customCodedBar71[num65], initiatedDataType);
		}
		for (int num66 = 0; num66 < 4; num66++)
		{
			BarSlots[71].InitiateCode(num66, editorData2.customCodedBar72[num66], initiatedDataType);
		}
		for (int num67 = 0; num67 < 4; num67++)
		{
			BarSlots[72].InitiateCode(num67, editorData2.customCodedBar73[num67], initiatedDataType);
		}
		for (int num68 = 0; num68 < 4; num68++)
		{
			BarSlots[73].InitiateCode(num68, editorData2.customCodedBar74[num68], initiatedDataType);
		}
		for (int num69 = 0; num69 < 4; num69++)
		{
			BarSlots[74].InitiateCode(num69, editorData2.customCodedBar75[num69], initiatedDataType);
		}
		for (int num70 = 0; num70 < 4; num70++)
		{
			BarSlots[75].InitiateCode(num70, editorData2.customCodedBar76[num70], initiatedDataType);
		}
		for (int num71 = 0; num71 < 4; num71++)
		{
			BarSlots[76].InitiateCode(num71, editorData2.customCodedBar77[num71], initiatedDataType);
		}
		for (int num72 = 0; num72 < 4; num72++)
		{
			BarSlots[77].InitiateCode(num72, editorData2.customCodedBar78[num72], initiatedDataType);
		}
		for (int num73 = 0; num73 < 4; num73++)
		{
			BarSlots[78].InitiateCode(num73, editorData2.customCodedBar79[num73], initiatedDataType);
		}
		for (int num74 = 0; num74 < 4; num74++)
		{
			BarSlots[79].InitiateCode(num74, editorData2.customCodedBar80[num74], initiatedDataType);
		}
		for (int num75 = 0; num75 < 4; num75++)
		{
			BarSlots[80].InitiateCode(num75, editorData2.customCodedBar81[num75], initiatedDataType);
		}
		for (int num76 = 0; num76 < 4; num76++)
		{
			BarSlots[81].InitiateCode(num76, editorData2.customCodedBar82[num76], initiatedDataType);
		}
		for (int num77 = 0; num77 < 4; num77++)
		{
			BarSlots[82].InitiateCode(num77, editorData2.customCodedBar83[num77], initiatedDataType);
		}
		for (int num78 = 0; num78 < 4; num78++)
		{
			BarSlots[83].InitiateCode(num78, editorData2.customCodedBar84[num78], initiatedDataType);
		}
		for (int num79 = 0; num79 < 4; num79++)
		{
			BarSlots[84].InitiateCode(num79, editorData2.customCodedBar85[num79], initiatedDataType);
		}
		for (int num80 = 0; num80 < 4; num80++)
		{
			BarSlots[85].InitiateCode(num80, editorData2.customCodedBar86[num80], initiatedDataType);
		}
		for (int num81 = 0; num81 < 4; num81++)
		{
			BarSlots[86].InitiateCode(num81, editorData2.customCodedBar87[num81], initiatedDataType);
		}
		for (int num82 = 0; num82 < 4; num82++)
		{
			BarSlots[87].InitiateCode(num82, editorData2.customCodedBar88[num82], initiatedDataType);
		}
		for (int num83 = 0; num83 < 4; num83++)
		{
			BarSlots[88].InitiateCode(num83, editorData2.customCodedBar89[num83], initiatedDataType);
		}
		for (int num84 = 0; num84 < 4; num84++)
		{
			BarSlots[89].InitiateCode(num84, editorData2.customCodedBar90[num84], initiatedDataType);
		}
		for (int num85 = 0; num85 < 4; num85++)
		{
			BarSlots[90].InitiateCode(num85, editorData2.customCodedBar91[num85], initiatedDataType);
		}
		for (int num86 = 0; num86 < 4; num86++)
		{
			BarSlots[91].InitiateCode(num86, editorData2.customCodedBar92[num86], initiatedDataType);
		}
		for (int num87 = 0; num87 < 4; num87++)
		{
			BarSlots[92].InitiateCode(num87, editorData2.customCodedBar93[num87], initiatedDataType);
		}
		for (int num88 = 0; num88 < 4; num88++)
		{
			BarSlots[93].InitiateCode(num88, editorData2.customCodedBar94[num88], initiatedDataType);
		}
		for (int num89 = 0; num89 < 4; num89++)
		{
			BarSlots[94].InitiateCode(num89, editorData2.customCodedBar95[num89], initiatedDataType);
		}
		for (int num90 = 0; num90 < 4; num90++)
		{
			BarSlots[95].InitiateCode(num90, editorData2.customCodedBar96[num90], initiatedDataType);
		}
		for (int num91 = 0; num91 < 4; num91++)
		{
			BarSlots[96].InitiateCode(num91, editorData2.customCodedBar97[num91], initiatedDataType);
		}
		for (int num92 = 0; num92 < 4; num92++)
		{
			BarSlots[97].InitiateCode(num92, editorData2.customCodedBar98[num92], initiatedDataType);
		}
		for (int num93 = 0; num93 < 4; num93++)
		{
			BarSlots[98].InitiateCode(num93, editorData2.customCodedBar99[num93], initiatedDataType);
		}
		for (int num94 = 0; num94 < 4; num94++)
		{
			BarSlots[99].InitiateCode(num94, editorData2.customCodedBar100[num94], initiatedDataType);
		}
		for (int num95 = 0; num95 < 4; num95++)
		{
			BarSlots[100].InitiateCode(num95, editorData2.customCodedBar101[num95], initiatedDataType);
		}
		for (int num96 = 0; num96 < 4; num96++)
		{
			BarSlots[101].InitiateCode(num96, editorData2.customCodedBar102[num96], initiatedDataType);
		}
		for (int num97 = 0; num97 < 4; num97++)
		{
			BarSlots[102].InitiateCode(num97, editorData2.customCodedBar103[num97], initiatedDataType);
		}
		for (int num98 = 0; num98 < 4; num98++)
		{
			BarSlots[103].InitiateCode(num98, editorData2.customCodedBar104[num98], initiatedDataType);
		}
		for (int num99 = 0; num99 < 4; num99++)
		{
			BarSlots[104].InitiateCode(num99, editorData2.customCodedBar105[num99], initiatedDataType);
		}
		for (int num100 = 0; num100 < 4; num100++)
		{
			BarSlots[105].InitiateCode(num100, editorData2.customCodedBar106[num100], initiatedDataType);
		}
		for (int num101 = 0; num101 < 4; num101++)
		{
			BarSlots[106].InitiateCode(num101, editorData2.customCodedBar107[num101], initiatedDataType);
		}
		for (int num102 = 0; num102 < 4; num102++)
		{
			BarSlots[107].InitiateCode(num102, editorData2.customCodedBar108[num102], initiatedDataType);
		}
		RefreshCursor();
	}

	public void RefreshBars()
	{
		for (int i = 0; i < LvlEditor.dir.GetBarsLength(); i++)
		{
			BarSlots[i].Show(i);
		}
		for (int j = LvlEditor.dir.GetBarsLength(); j < BarSlots.Length; j++)
		{
			BarSlots[j].Hide();
		}
		if (activeBarSlotNum + 1 > LvlEditor.dir.GetBarsLength())
		{
			activeBarSlotNum = LvlEditor.dir.GetBarsLength() - 1;
			activeBeatSlotNum = 0;
			RefreshCursor();
		}
	}

	private void RefreshCursor()
	{
		Wrapper.SetLocalPosition(wrapperInitLocalX - (float)activeBarSlotNum * 9.34f - (float)activeBeatSlotNum * 2.335f, wrapperInitLocalY);
	}

	public void SetCodeOnBeat(string codeAdded, char dataType)
	{
		BarSlots[activeBarSlotNum].SetCode(activeBeatSlotNum, codeAdded, dataType);
	}

	public void RemoveCodeOnBeat(char charType)
	{
		BarSlots[activeBarSlotNum].RemoveCode(activeBeatSlotNum, charType);
	}

	public void IncreaseBeat()
	{
		CancelCoroutine(changingBeat);
		changingBeat = StartCoroutine(IncreasingBeat());
	}

	private IEnumerator IncreasingBeat()
	{
		if (CheckIsLastBeatActive())
		{
			yield break;
		}
		activeBeatSlotNum++;
		if (activeBeatSlotNum >= 4)
		{
			activeBarSlotNum++;
			activeBeatSlotNum = 0;
		}
		Wrapper.MoveToLocalTarget(new Vector3(wrapperInitLocalX - (float)activeBarSlotNum * 9.34f - (float)activeBeatSlotNum * 2.335f, wrapperInitLocalY, 0f), speed, isEasingIn: false);
		yield return new WaitForSeconds(0.33f);
		int numMoves = 0;
		while (ControlHandler.mgr.CheckIsRightPressing() && !CheckIsLastBeatActive())
		{
			EditorUI.env.PlayToggleSfx();
			activeBeatSlotNum++;
			if (activeBeatSlotNum >= 4)
			{
				activeBarSlotNum++;
				activeBeatSlotNum = 0;
			}
			Wrapper.MoveToLocalTarget(new Vector3(wrapperInitLocalX - (float)activeBarSlotNum * 9.34f - (float)activeBeatSlotNum * 2.335f, wrapperInitLocalY, 0f), speed, isEasingIn: false);
			numMoves++;
			if (numMoves >= 16)
			{
				yield return new WaitForSeconds(0.02f);
			}
			else if (numMoves >= 4)
			{
				yield return new WaitForSeconds(0.033f);
			}
			else
			{
				yield return new WaitForSeconds(0.067f);
			}
			yield return null;
		}
	}

	public void DecreaseBeat()
	{
		CancelCoroutine(changingBeat);
		changingBeat = StartCoroutine(DecreasingBeat());
	}

	private IEnumerator DecreasingBeat()
	{
		if (CheckIsFirstBeatActive())
		{
			yield break;
		}
		activeBeatSlotNum--;
		if (activeBeatSlotNum < 0)
		{
			activeBarSlotNum--;
			activeBeatSlotNum = 3;
		}
		Wrapper.MoveToLocalTarget(new Vector3(wrapperInitLocalX - (float)activeBarSlotNum * 9.34f - (float)activeBeatSlotNum * 2.335f, wrapperInitLocalY, 0f), speed, isEasingIn: false);
		yield return new WaitForSeconds(0.33f);
		int numMoves = 0;
		while (ControlHandler.mgr.CheckIsLeftPressing() && !CheckIsFirstBeatActive())
		{
			EditorUI.env.PlayToggleSfx();
			activeBeatSlotNum--;
			if (activeBeatSlotNum < 0)
			{
				activeBarSlotNum--;
				activeBeatSlotNum = 3;
			}
			Wrapper.MoveToLocalTarget(new Vector3(wrapperInitLocalX - (float)activeBarSlotNum * 9.34f - (float)activeBeatSlotNum * 2.335f, wrapperInitLocalY, 0f), speed, isEasingIn: false);
			numMoves++;
			if (numMoves >= 16)
			{
				yield return new WaitForSeconds(0.02f);
			}
			else if (numMoves >= 4)
			{
				yield return new WaitForSeconds(0.033f);
			}
			else
			{
				yield return new WaitForSeconds(0.067f);
			}
			yield return null;
		}
	}

	public void IncreaseBar()
	{
		CancelCoroutine(changingBeat);
		changingBeat = StartCoroutine(IncreasingBar());
	}

	private IEnumerator IncreasingBar()
	{
		if (CheckIsLastBarActive())
		{
			yield break;
		}
		activeBarSlotNum++;
		Wrapper.MoveToLocalTarget(new Vector3(wrapperInitLocalX - (float)activeBarSlotNum * 9.34f - (float)activeBeatSlotNum * 2.335f, wrapperInitLocalY, 0f), speed, isEasingIn: false);
		yield return new WaitForSeconds(0.33f);
		int numMoves = 0;
		while (ControlHandler.mgr.CheckIsUpPressing() && !CheckIsLastBarActive())
		{
			EditorUI.env.PlayToggleSfx();
			activeBarSlotNum++;
			Wrapper.MoveToLocalTarget(new Vector3(wrapperInitLocalX - (float)activeBarSlotNum * 9.34f - (float)activeBeatSlotNum * 2.335f, wrapperInitLocalY, 0f), speed, isEasingIn: false);
			numMoves++;
			if (numMoves >= 16)
			{
				yield return new WaitForSeconds(0.02f);
			}
			else if (numMoves >= 4)
			{
				yield return new WaitForSeconds(0.033f);
			}
			else
			{
				yield return new WaitForSeconds(0.067f);
			}
			yield return null;
		}
	}

	public void DecreaseBar()
	{
		CancelCoroutine(changingBeat);
		changingBeat = StartCoroutine(DecreasingBar());
	}

	private IEnumerator DecreasingBar()
	{
		if (CheckIsFirstBarActive())
		{
			yield break;
		}
		activeBarSlotNum--;
		Wrapper.MoveToLocalTarget(new Vector3(wrapperInitLocalX - (float)activeBarSlotNum * 9.34f - (float)activeBeatSlotNum * 2.335f, wrapperInitLocalY, 0f), speed, isEasingIn: false);
		yield return new WaitForSeconds(0.33f);
		int numMoves = 0;
		while (ControlHandler.mgr.CheckIsDownPressing() && !CheckIsFirstBarActive())
		{
			EditorUI.env.PlayToggleSfx();
			activeBarSlotNum--;
			Wrapper.MoveToLocalTarget(new Vector3(wrapperInitLocalX - (float)activeBarSlotNum * 9.34f - (float)activeBeatSlotNum * 2.335f, wrapperInitLocalY, 0f), speed, isEasingIn: false);
			numMoves++;
			if (numMoves >= 16)
			{
				yield return new WaitForSeconds(0.02f);
			}
			else if (numMoves >= 4)
			{
				yield return new WaitForSeconds(0.033f);
			}
			else
			{
				yield return new WaitForSeconds(0.067f);
			}
			yield return null;
		}
	}

	public static void ResetActiveBeatAndBar()
	{
		activeBarSlotNum = 0;
		activeBeatSlotNum = 0;
	}

	private bool CheckIsFirstBeatActive()
	{
		if (activeBarSlotNum == 0)
		{
			return activeBeatSlotNum == 0;
		}
		return false;
	}

	private bool CheckIsFirstBarActive()
	{
		return activeBarSlotNum == 0;
	}

	private bool CheckIsLastBeatActive()
	{
		if (activeBarSlotNum == LvlEditor.dir.GetBarsLength() - 1)
		{
			return activeBeatSlotNum == 3;
		}
		return false;
	}

	private bool CheckIsLastBarActive()
	{
		return activeBarSlotNum == LvlEditor.dir.GetBarsLength() - 1;
	}

	public bool CheckIsActiveBeatDenied()
	{
		return BarSlots[activeBarSlotNum].CheckIsBeatSlotDenied(activeBeatSlotNum);
	}

	public int GetPhraseNum()
	{
		return (int)Mathf.Ceil((float)(activeBarSlotNum + 1) / 8f);
	}

	public int GetBarNum()
	{
		return activeBarSlotNum + 1 - (GetPhraseNum() - 1) * 8;
	}

	public int GetBeatNum()
	{
		return activeBeatSlotNum + 1;
	}

	public int GetActiveBarSlotNum()
	{
		return activeBarSlotNum;
	}

	public string GetCodeOnBeat()
	{
		return BarSlots[activeBarSlotNum].GetCode(activeBeatSlotNum);
	}
}
