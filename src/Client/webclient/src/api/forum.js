import { $host } from "./axios";

export const fetchQuestions = async (page = 1, pageSize = 10) => {
  const { data } = await $host.get('Forume/questions', {
    params: {
      PageNumber: page,
      PageSize: pageSize,
    },
  });
  return data;
};