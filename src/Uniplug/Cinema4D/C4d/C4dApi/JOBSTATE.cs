//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 3.0.8
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace C4d {

public enum JOBSTATE {
  JOBSTATE_IDLE = 1000,
  JOBSTATE_PREPARING_RUNNING,
  JOBSTATE_PREPARING_FAILED,
  JOBSTATE_PREPARING_OK,
  JOBSTATE_RENDER_RUNNING,
  EX_JOBSTATE_RENDER_PAUSED,
  JOBSTATE_RENDER_OK,
  JOBSTATE_RENDER_FAILED,
  JOBSTATE_ALLOCATESPACE_RUNNING,
  JOBSTATE_ALLOCATESPACE_OK,
  JOBSTATE_ALLOCATESPACE_FAILED,
  JOBSTATE_DOWNLOAD_RUNNING,
  JOBSTATE_DOWNLOAD_OK,
  JOBSTATE_DOWNLOAD_FAILED,
  JOBSTATE_ASSEMBLE_RUNNING,
  JOBSTATE_ASSEMBLE_OK,
  JOBSTATE_ASSEMBLE_FAILED,
  JOBSTATE_STOPPED,
  JOBSTATE_QUEUED
}

}