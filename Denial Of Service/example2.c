#include <errno.h>
#include <limits.h>
int i;
char inLine[64];
cin >> inLine;
char *endptr;
errno = 0;
long result = strtol(inLine, &endptr, 10);
if (errno == ERANGE || result > INT_MAX || result < INT_MIN || *endptr != '\0') {
    return -1;
}
i = (int)result;
sleep(i);
