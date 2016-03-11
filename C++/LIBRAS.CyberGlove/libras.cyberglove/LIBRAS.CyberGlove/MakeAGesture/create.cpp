#define _WIN32_WINNT 0x0400
#include <iostream>
#include <fstream>
#include <vhtBase.h>
#include <conio.h>

using namespace std;
using std::cout;

int MAKE_A_GESTURE = 115;
int END_GESTURES = 109;
int QUANTITY_SENSORS = 23;

int main(int argc, char *argv[])
{
	vhtIOConn *gloveDict;
	vhtCyberGlove *glove;
	vhtIOConn *trackerDict;
	vhtTracker *tracker;

	try
	{
		gloveDict = vhtIOConn::getDefault( vhtIOConn::glove );
		glove = new vhtCyberGlove(gloveDict);

#if defined( USE_REAL_TRACKER )
        trackerDict = vhtIOConn::getDefault( vhtIOConn::tracker );
        tracker = new vhtTracker( trackerDict );
#else
        tracker = new vhtTrackerEmulator();
#endif
	}
	catch (vhtBaseException *e)
	{
		printf("Error: %s\nPress <enter> to exit.\n", e->getMessage());
		getchar();
		return 0;
	}

	vhtTransform3D trackerXForm;
	vhtVector3d	position;
	vhtQuaternion  orientation;
	vhtVector3d	axis;
	double baseT = glove->getLastUpdateTime();

	while(true) {

		glove->update();
		tracker->update();

		cout << "Make the Gesture\n\n";

		int character = getch();

		ofstream file;

		if(character == MAKE_A_GESTURE) {
			file.open("gesture.txt");

			for( int sensor = 0; sensor < QUANTITY_SENSORS; sensor++ ) {
				file << glove->getData(sensor);
				cout << glove->getData(sensor);
				
				if(sensor != QUANTITY_SENSORS - 1) {
					file << " ";
					file.close();
				}
			}

		} else if(character == END_GESTURES) {
			file.close();
			return 1;
		}

		for( int sensor = 0; sensor < QUANTITY_SENSORS; sensor++ ) {
			file << glove->getData(sensor);
			cout << glove->getData(sensor);
				
			if(sensor != QUANTITY_SENSORS - 1) {
				file << " ";
				file.close();
			}
		}

		Sleep(100);
	}

	return 0;
}