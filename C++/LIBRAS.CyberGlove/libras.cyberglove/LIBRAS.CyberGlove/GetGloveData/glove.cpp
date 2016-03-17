#define _WIN32_WINNT 0x0400
#include <iostream>
#include <fstream>
#include <vhtBase.h>
#include <conio.h>

using namespace std;
using std::cout;

int SAVE = 115;
int QUANTITY_SENSORS = 22;
int QUANTITY_POSES = 10;

int main(int argc, char *argv[])
{
	int quantityPoses = 0;

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

	ofstream file;
	file.open("data.txt");

	if(!file) {
		cout << "File to create file\n\n\n";
		return -1;
	}

	cout << "Make the Gesture\n\n";

	while(true) {

		glove->update();
		tracker->update();

		int character = getch();

		cout << "\nPose number: " << quantityPoses+1 << "\n\n";
		cout << "Time: " << glove->getLastUpdateTime() - baseT << "\n\n";
		cout << "Glove: \n";

		/*for(int finger = 0; finger < GHM::nbrFingers; finger++) {
			cout << finger << " ";
			file << finger << " ";

			for(int joint = 0; joint < GHM::nbrJoints; joint++) {
				cout << glove->getData( (GHM::Fingers)finger, (GHM::Joints)joint ) << " ";
				file << glove->getData( (GHM::Fingers)finger, (GHM::Joints)joint ) << " ";
			}
			cout << "\n";
			file << "\n";
		}*/

		for( int sensor = 0; sensor < QUANTITY_SENSORS; sensor++ ) {
			cout <<  glove->getData(sensor) << " ";
		}
		cout << "\n\n";
	
		if(character == SAVE) {
			quantityPoses++;

			for( int sensor = 0; sensor < QUANTITY_SENSORS; sensor++ ) {
				file <<  glove->getData(sensor);
				
				if(sensor != QUANTITY_SENSORS - 1) {
					file << " ";
				}
			}

			if(quantityPoses == QUANTITY_POSES) {
				file.close();
				return 0;
			} else {
				file << "\n";
			}
		}

		Sleep(100);
	}

	return 0;
}