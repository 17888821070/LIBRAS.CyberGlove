#define _WIN32_WINNT 0x0400
#include <iostream>
#include <fstream>
#include <vhtBase.h>
#include <conio.h>

using namespace std;
using std::cout;

int SAVE_A_GESTURE = 115; // s
int QUIT_GESTURES = 113;  // n
int QUANTITY_SENSORS = 22;

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

	ofstream file;
	file.open("C:/LIBRAS.CyberGlove/gesture.txt");

	while(true) {

		cout << "Save gesture for detection?\n\n";
		int character = getch();

		glove->update();
		tracker->update();

		for(int sensor = 0; sensor < QUANTITY_SENSORS; sensor++) {
			cout << glove->getData(sensor) << " ";
		}

		if(character == SAVE_A_GESTURE) {
			for(int sensor = 0; sensor < QUANTITY_SENSORS; sensor++) {
				file << glove->getData(sensor);
				
				if(sensor < QUANTITY_SENSORS - 1) {
					file << " ";
				} else {
					file << "\n";
					cout << "\n\nGesture saved!\n\n";
				}
			}

		} else if(character == QUIT_GESTURES) {
			file.close();
			system("start C:\\LIBRAS.CyberGlove\\DetectGestures\\LIBRAS.CyberGlove.exe");
			
			return 0;
		} else {
			cout << "\n\nGesture not saved!\n\n";
		}

		Sleep(100);
	}

	return 0;
}