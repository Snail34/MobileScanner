#include "pch.h"
#include "OpencvImageProcess.h"
#include <opencv2\core.hpp>
#include <opencv2\imgproc\imgproc.hpp>
#include <robuffer.h>

using namespace cv;
using namespace OpenCVRT;
using namespace Platform;

OpencvImageProcess::OpencvImageProcess()
{
	
}

WriteableBitmap^ OpencvImageProcess::GetImageCorners(WriteableBitmap^ source)
{
	//WriteableBitmap^ bitmap = ref new WriteableBitmap(source->PixelWidth, source->PixelHeight);

	//

	byte* bytes = this->GetPointerToPixelData(source->PixelBuffer);

	byte* temp = bytes;

	Mat src;

	Size src_size(source->PixelWidth, source->PixelHeight);

	src = Mat::zeros(src_size, CV_8UC4);

	src.data = temp;

	Mat src_gray;

	src_gray = Mat::zeros(src.size(), CV_8UC4);
	
	Mat dst, dst_norm, dst_norm_scaled;

	dst = Mat::zeros(src.size(), CV_32FC1);

	int blockSize = 2;

	int apertureSize = 3;

	double k = 0.04;

	cvtColor(src, src_gray, CV_BGR2GRAY);

	cornerHarris(src_gray, dst, blockSize, apertureSize, k, BORDER_DEFAULT);

	normalize(dst, dst_norm, 0, 255, NORM_MINMAX, CV_32FC1, Mat());

	convertScaleAbs(dst_norm, dst_norm_scaled);

	for (int j = 0; j < dst_norm.rows; j++)
	{
		for (int i = 0; i < dst_norm.cols; i++)
		{
			if ((int)dst_norm.at<float>(j, i) > 150)
			{
				circle(dst_norm_scaled, Point(i, j), 5, Scalar(0), 2, 8, 0);
			}
		}
	}

	int size = dst_norm_scaled.total() * dst_norm_scaled.elemSize();

	byte* result = new byte[size];

	std::memcpy(result, dst_norm_scaled.data, size * sizeof(byte));

	Size s = dst_norm_scaled.size();

	for (int y = 0; y < s.height; y++)
	{
		for (int x = 0; x < s.width; x++)
		{
			bytes[(x + y * s.width) * 4] = result[(x + y * s.width)]; // B
			bytes[(x + y * s.width) * 4 + 1] = result[(x + y * s.width)]; // G
			bytes[(x + y * s.width) * 4 + 2] = result[(x + y * s.width)]; // R
			bytes[(x + y * s.width) * 4 + 3] = result[(x + y * s.width)]; // A
		}
	}

	return source;
}



byte* OpencvImageProcess::GetPointerToPixelData(IBuffer^ buffer)
{
	Object^ obj = buffer;
	ComPtr<IInspectable> insp(reinterpret_cast<IInspectable*>(obj));

	// Query the IBufferByteAccess interface.
	ComPtr<IBufferByteAccess> bufferByteAccess;
	ThrowIfFailed(insp.As(&bufferByteAccess));

	// Retrieve the buffer data.
	byte* pixels = nullptr;
	ThrowIfFailed(bufferByteAccess->Buffer(&pixels));
	return pixels;
}

inline void OpencvImageProcess::ThrowIfFailed(HRESULT hr)
{
	if (FAILED(hr))
	{
		//throw Exception::CreateException(hr);
	}
}