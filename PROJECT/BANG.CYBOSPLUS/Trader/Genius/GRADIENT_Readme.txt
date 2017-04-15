1. 개요
현재 종목이 상승추세 인지 아닌지 판단
BUY20

2. 수식
기울기 계산
	while (fgets(text, TEXTLENGTH, stdin) != NULL)
	{
		if (sscanf_s(text, "%lf %lf", &xi, &yi) == 2)
		{
			sxi += xi;
			syi += yi;

			sxiyi += xi*yi;
			sxi2 += xi*xi;

			++n;
		}
		else
		{
			fprintf(stderr, "바르지 않은 데이터입니다.: %s", text);
		}
	}

	if (n > 1)
	{
		// a1 이 기울기
		a0 = (sxi2*syi - sxiyi*sxi) / (n*sxi2 - sxi*sxi);
		a1 = (n*sxiyi - sxi*syi) / (n*sxi2 - sxi*sxi);

		printf("%1f\n%1f\n", a0, a1);
	}

3. 상세로직
	3-1. 분봉(1분) 기준 상승추세 여부 판단
		- 당일 9시부터 BUY20-M

	3-2. 일봉(1일) 기준 상승추세 여부 판단
		- 최근 1주일 BUY20-D7
		- 최근 2주일 BUY20-D14
		- 최근 1개월 BUY20-D30
		- 최근 2개월 BUY20-D60
		- 최근 3개월 BUY20-D90
		- 최근 6개월 BUY20-D180
	3-3. 기울기 적용
		- 30도 이내		0      < a <= 0.5774
		- 31~60도 사이  0.5774 < a <= 1.732
		- 61~90도 사이  1.732  < a