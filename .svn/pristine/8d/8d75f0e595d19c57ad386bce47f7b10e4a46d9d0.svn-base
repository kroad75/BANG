// Exam2-1.cpp : 콘솔 응용 프로그램에 대한 진입점을 정의합니다.
//

#include "stdafx.h"

#include <stdio.h>

#define TEXTLENGTH 4096
int _tmain(int argc, _TCHAR* argv[])
{
	char text[TEXTLENGTH];
	double xi, yi;
	double sxi = 0, syi = 0, sxiyi = 0, sxi2 = 0;
	double a0, a1;
	int n = 0;

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
		a0 = (sxi2*syi - sxiyi*sxi) / (n*sxi2 - sxi*sxi);
		a1 = (n*sxiyi - sxi*syi) / (n*sxi2 - sxi*sxi);

		printf("%1f\n%1f\n", a0, a1);
	}
	else
	{
		fprintf(stderr, "데이터가 부족합니다.\n");
	}


	return 0;
}

